#nullable enable
using System.Collections.Concurrent;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using Serilog;
using SnsMessageAttributeValue = Amazon.SimpleNotificationService.Model.MessageAttributeValue;

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus
{
    /// <summary>
    /// A bridge event bus used by the monolith to route events to/from the extracted
    /// Administration service via SNS/SQS, while keeping local in-memory routing
    /// for modules that still run in-process.
    /// </summary>
    public class SnsBridgeEventsBus : IEventsBus
    {
        private readonly InMemoryEventBusClient _inMemoryBus;
        private readonly ILogger _logger;
        private readonly SnsEventsBusConfiguration _configuration;
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IAmazonSQS _sqsClient;
        private readonly ConcurrentDictionary<string, string> _topicArnCache;
        private readonly ConcurrentDictionary<string, string> _queueUrlCache;
        private readonly HashSet<string> _remoteEventTypes;
        private readonly ConcurrentDictionary<string, List<IIntegrationEventHandler>> _remoteHandlers;
        private CancellationTokenSource? _consumingCts;
        private bool _disposed;

        public SnsBridgeEventsBus(
            ILogger logger,
            SnsEventsBusConfiguration configuration,
            IEnumerable<string> remoteEventTypes)
        {
            _inMemoryBus = new InMemoryEventBusClient(logger);
            _logger = logger;
            _configuration = configuration;
            _topicArnCache = new ConcurrentDictionary<string, string>();
            _queueUrlCache = new ConcurrentDictionary<string, string>();
            _remoteEventTypes = new HashSet<string>(remoteEventTypes);
            _remoteHandlers = new ConcurrentDictionary<string, List<IIntegrationEventHandler>>();

            var regionEndpoint = RegionEndpoint.GetBySystemName(configuration.Region);

            var snsConfig = new AmazonSimpleNotificationServiceConfig
            {
                RegionEndpoint = regionEndpoint
            };

            var sqsConfig = new AmazonSQSConfig
            {
                RegionEndpoint = regionEndpoint
            };

            if (!string.IsNullOrEmpty(configuration.ServiceUrl))
            {
                snsConfig.ServiceURL = configuration.ServiceUrl;
                sqsConfig.ServiceURL = configuration.ServiceUrl;
            }

            _snsClient = new AmazonSimpleNotificationServiceClient(snsConfig);
            _sqsClient = new AmazonSQSClient(sqsConfig);
        }

        public async Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            var eventType = @event.GetType().FullName;
            if (eventType == null)
            {
                return;
            }

            if (_remoteEventTypes.Contains(eventType))
            {
                _logger.Information("Publishing {EventType} to SNS (remote Administration service)", eventType);
                await PublishToSns(@event, eventType);
            }
            else
            {
                await _inMemoryBus.Publish(@event);
            }
        }

        public void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent
        {
            var eventType = typeof(T).FullName;
            if (eventType == null)
            {
                return;
            }

            if (_remoteEventTypes.Contains(eventType))
            {
                _logger.Information(
                    "Subscribing to {EventType} via SQS (from remote Administration service)",
                    eventType);

                _remoteHandlers.AddOrUpdate(
                    eventType,
                    _ => new List<IIntegrationEventHandler> { handler },
                    (_, existing) =>
                    {
                        existing.Add(handler);
                        return existing;
                    });
            }
            else
            {
                _inMemoryBus.Subscribe(handler);
            }
        }

        public void StartConsuming()
        {
            _consumingCts = new CancellationTokenSource();

            foreach (var eventType in _remoteHandlers.Keys)
            {
                _ = Task.Run(() => ConsumeMessages(eventType, _consumingCts.Token));
            }

            _logger.Information(
                "SnsBridgeEventsBus started. Remote event types: {Count}, SQS consumers: {ConsumerCount}",
                _remoteEventTypes.Count,
                _remoteHandlers.Count);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _consumingCts?.Cancel();
            _consumingCts?.Dispose();
            _inMemoryBus.Dispose();
            _snsClient.Dispose();
            _sqsClient.Dispose();
        }

        private async Task PublishToSns<T>(T @event, string eventType)
            where T : IntegrationEvent
        {
            var topicArn = await GetOrCreateTopicArn(eventType);
            var messageBody = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            var publishRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = messageBody,
                MessageAttributes = new Dictionary<string, SnsMessageAttributeValue>
                {
                    ["EventType"] = new SnsMessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = eventType
                    }
                }
            };

            await _snsClient.PublishAsync(publishRequest);
        }

        private async Task ConsumeMessages(string eventType, CancellationToken cancellationToken)
        {
            var queueUrl = await GetOrCreateQueueUrl(eventType);
            var topicArn = await GetOrCreateTopicArn(eventType);
            await EnsureSubscription(topicArn, queueUrl);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var receiveRequest = new ReceiveMessageRequest
                    {
                        QueueUrl = queueUrl,
                        MaxNumberOfMessages = 10,
                        WaitTimeSeconds = 20,
                        MessageAttributeNames = new List<string> { "All" }
                    };

                    var response = await _sqsClient.ReceiveMessageAsync(receiveRequest, cancellationToken);

                    foreach (var message in response.Messages)
                    {
                        try
                        {
                            await ProcessMessage(eventType, message.Body);

                            await _sqsClient.DeleteMessageAsync(
                                queueUrl,
                                message.ReceiptHandle,
                                cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(
                                ex,
                                "Error processing bridged SQS message for {EventType}",
                                eventType);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error receiving bridged SQS messages for {EventType}", eventType);
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }

        private async Task ProcessMessage(string eventType, string messageBody)
        {
            var snsMessage = JsonConvert.DeserializeObject<SnsMessageEnvelope>(messageBody);
            var actualMessage = snsMessage?.Message ?? messageBody;

            var @event = JsonConvert.DeserializeObject(actualMessage, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (@event == null)
            {
                _logger.Warning("Failed to deserialize bridged message for event type {EventType}", eventType);
                return;
            }

            if (!_remoteHandlers.TryGetValue(eventType, out var handlers))
            {
                return;
            }

            foreach (var handler in handlers)
            {
                var handleMethod = handler.GetType().GetMethod("Handle");
                if (handleMethod != null)
                {
                    var task = (Task?)handleMethod.Invoke(handler, new[] { @event });
                    if (task != null)
                    {
                        await task;
                    }
                }
            }
        }

        private async Task<string> GetOrCreateTopicArn(string eventType)
        {
            if (_topicArnCache.TryGetValue(eventType, out var cachedArn))
            {
                return cachedArn;
            }

            var topicName = SanitizeTopicName(eventType);

            if (!string.IsNullOrEmpty(_configuration.TopicArnPrefix))
            {
                var arn = $"{_configuration.TopicArnPrefix}:{topicName}";
                _topicArnCache.TryAdd(eventType, arn);
                return arn;
            }

            var createResponse = await _snsClient.CreateTopicAsync(new CreateTopicRequest
            {
                Name = topicName
            });

            _topicArnCache.TryAdd(eventType, createResponse.TopicArn);
            return createResponse.TopicArn;
        }

        private async Task<string> GetOrCreateQueueUrl(string eventType)
        {
            if (_queueUrlCache.TryGetValue(eventType, out var cachedUrl))
            {
                return cachedUrl;
            }

            var queueName = SanitizeTopicName(eventType) + "-monolith-queue";

            if (!string.IsNullOrEmpty(_configuration.SqsQueueUrlPrefix))
            {
                var url = $"{_configuration.SqsQueueUrlPrefix}/{queueName}";
                _queueUrlCache.TryAdd(eventType, url);
                return url;
            }

            var createResponse = await _sqsClient.CreateQueueAsync(new CreateQueueRequest
            {
                QueueName = queueName
            });

            _queueUrlCache.TryAdd(eventType, createResponse.QueueUrl);
            return createResponse.QueueUrl;
        }

        private async Task EnsureSubscription(string topicArn, string queueUrl)
        {
            var queueArnResponse = await _sqsClient.GetQueueAttributesAsync(
                new GetQueueAttributesRequest
                {
                    QueueUrl = queueUrl,
                    AttributeNames = new List<string> { "QueueArn" }
                });

            var queueArn = queueArnResponse.QueueARN;

            await _snsClient.SubscribeAsync(new SubscribeRequest
            {
                TopicArn = topicArn,
                Protocol = "sqs",
                Endpoint = queueArn
            });
        }

        private static string SanitizeTopicName(string eventType)
        {
            return eventType
                .Replace(".", "-")
                .Replace("+", "-");
        }

        private sealed class SnsMessageEnvelope
        {
            public string? Message { get; set; }

            public string? MessageId { get; set; }

            public string? TopicArn { get; set; }
        }
    }
}
