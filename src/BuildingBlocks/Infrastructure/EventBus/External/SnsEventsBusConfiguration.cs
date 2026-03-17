#nullable enable
namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus
{
    public class SnsEventsBusConfiguration
    {
        public string Region { get; set; } = "us-east-1";

        public string TopicArnPrefix { get; set; } = string.Empty;

        public string? ServiceUrl { get; set; }

        public string? SqsQueueUrlPrefix { get; set; }
    }
}
