using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.EventBus;

namespace CompanyName.MyMeetings.Modules.Administration.Infrastructure.Configuration.EventsBus
{
    internal class EventsBusModule : BuildingBlocks.Infrastructure.Configuration.EventsBusModule
    {
        public EventsBusModule(IEventsBus eventsBus)
            : base(eventsBus)
        {
        }
    }
}
