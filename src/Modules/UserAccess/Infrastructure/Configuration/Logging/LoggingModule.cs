namespace CompanyName.MyMeetings.Modules.UserAccess.Infrastructure.Configuration.Logging
{
    internal class LoggingModule : BuildingBlocks.Infrastructure.Configuration.LoggingModule
    {
        internal LoggingModule(Serilog.ILogger logger)
            : base(logger)
        {
        }
    }
}
