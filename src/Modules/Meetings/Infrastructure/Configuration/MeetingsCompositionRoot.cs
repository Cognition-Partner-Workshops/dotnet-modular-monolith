using Autofac;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration;

namespace CompanyName.MyMeetings.Modules.Meetings.Infrastructure.Configuration
{
    internal static class MeetingsCompositionRoot
    {
        private static readonly BaseCompositionRoot _inner = new();

        internal static BaseCompositionRoot Instance => _inner;

        internal static void SetContainer(IContainer container)
        {
            _inner.SetContainer(container);
        }

        internal static ILifetimeScope BeginLifetimeScope()
        {
            return _inner.BeginLifetimeScope();
        }
    }
}
