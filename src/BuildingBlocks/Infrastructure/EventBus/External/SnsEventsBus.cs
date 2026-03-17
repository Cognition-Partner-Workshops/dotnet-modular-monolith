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
    public class SnsEventsBus : IEventsBus
    {
        private readonly ILogger _logger;
        private readonly SnsEventsBusConfiguration _configuration;
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IAmazonSQS _sqsClient;
        private readonly ConcurrentDictionary<string, string> _topicArnCache;
        private readonly ConcurrentDictionary<string, List<IIntegrationEventHandler>> _handlers;
        private readonly ConcurrentDictionary<string, string> _queueUrlCache;
        private CancellationTokenSource? _consumingCts;
        private bool _disposed;

        public SnsEventsBus(ILogger logger, SnsEventsBusConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _topicArnCache = new ConcurrentDictionary<string, string>();
            _handlers = new ConcurrentDictionary<string, List<IIntegrationEventHandler>>();
            _queueUrlCache = new ConcurrentDictionary<string, string>();

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

            _logger.Information("Publishing integration event {EventType} to SNS", eventType);

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

            _logger.Information("Published integration event {EventType} to SNS topic {TopicArn}", eventType, topicArn);
        }

        public void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent
        {
            var eventType = typeof(T).FullName;
            if (eventType == null)
            {
                return;
            }

            _logger.Information("Subscribing to integration event {EventType} via SQS", eventType);

            _handlers.AddOrUpdate(
                eventType,
                _ => new List<IIntegrationEventHandler> { handler },
                (_, existing) =>
                {
                    existing.Add(handler);
                    return existing;
                });
        }

        public void StartConsuming()
        {
            _consumingCts = new CancellationTokenSource();

            foreach (var eventType in _handlers.Keys)
            {
                _ = Task.Run(() => ConsumeMessages(eventType, _consumingCts.Token));
            }

            _logger.Information("SnsEventsBus started consuming from {QueueCount} event type(s)", _handlers.Count);
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
            _snsClient.Dispose();
            _sqsClient.Dispose();
        }

        private async Task ConsumeMessages(string eventType, CancellationToken cancellationToken)
        {
            var queueUrl = await GetOrCreateQueueUrl(eventType);
            var topicArn = await GetOrCreateTopicArn(eventType);
            await EnsureSubscription(topicArn, queueUrl);

            _logger.Information(
                "Starting SQS consumer for event {EventType} on queue {QueueUrl}",
                eventType,
                queueUrl);

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
                                "Error processing SQS message for event {EventType}. MessageId: {MessageId}",
                                eventType,
                                message.MessageId);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error receiving SQS messages for event {EventType}", eventType);
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }

        private async Task ProcessMessage(string eventType, string messageBody)
        {
            // SNS wraps the message in an envelope; extract the actual message
            var snsMessage = JsonConvert.DeserializeObject<SnsMessageEnvelope>(messageBody);
            var actualMessage = snsMessage?.Message ?? messageBody;

            var @event = JsonConvert.DeserializeObject(actualMessage, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (@event == null)
            {
                _logger.Warning("Failed to deserialize message for event type {EventType}", eventType);
                return;
            }

            if (!_handlers.TryGetValue(eventType, out var handlers))
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

            var queueName = SanitizeTopicName(eventType) + "-queue";

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

            _logger.Information(
                "Ensured SNS subscription: topic {TopicArn} -> queue {QueueArn}",
                topicArn,
                queueArn);
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
