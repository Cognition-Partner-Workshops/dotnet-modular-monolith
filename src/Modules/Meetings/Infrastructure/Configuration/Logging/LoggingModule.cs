using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration;

namespace CompanyName.MyMeetings.Modules.Meetings.Infrastructure.Configuration.Logging
{
    internal class LoggingModule : BuildingBlocks.Infrastructure.Configuration.LoggingModule
    {
        internal LoggingModule(Serilog.ILogger logger)
            : base(logger)
        {
        }
    }
}
