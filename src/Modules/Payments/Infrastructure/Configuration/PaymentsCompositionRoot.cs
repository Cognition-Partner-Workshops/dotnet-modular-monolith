using Autofac;
using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration;

namespace CompanyName.MyMeetings.Modules.Payments.Infrastructure.Configuration
{
    public static class PaymentsCompositionRoot
    {
        private static readonly BaseCompositionRoot _inner = new();

        internal static BaseCompositionRoot Instance => _inner;

        public static void SetContainer(IContainer container)
        {
            _inner.SetContainer(container);
        }

        public static ILifetimeScope BeginLifetimeScope()
        {
            return _inner.BeginLifetimeScope();
        }
    }
}
