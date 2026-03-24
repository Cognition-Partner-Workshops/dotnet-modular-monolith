using Autofac;

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration
{
    public class BaseCompositionRoot
    {
        private IContainer _container;

        public void SetContainer(IContainer container)
        {
            _container = container;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
