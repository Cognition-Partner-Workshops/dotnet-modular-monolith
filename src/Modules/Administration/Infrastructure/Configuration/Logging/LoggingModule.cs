namespace CompanyName.MyMeetings.Modules.Administration.Infrastructure.Configuration.Logging
{
    internal class LoggingModule : BuildingBlocks.Infrastructure.Configuration.LoggingModule
    {
        internal LoggingModule(Serilog.ILogger logger)
            : base(logger)
        {
        }
    }
}
