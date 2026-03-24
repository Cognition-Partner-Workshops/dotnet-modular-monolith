using Autofac;
using MediatR;

namespace CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration
{
    public class BaseCommandsExecutor
    {
        private readonly BaseCompositionRoot _compositionRoot;

        public BaseCommandsExecutor(BaseCompositionRoot compositionRoot)
        {
            _compositionRoot = compositionRoot;
        }

        public async Task Execute(IRequest command)
        {
            using (var scope = _compositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();
                await mediator.Send(command);
            }
        }

        public async Task<TResult> Execute<TResult>(IRequest<TResult> command)
        {
            using (var scope = _compositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();
                return await mediator.Send(command);
            }
        }
    }
}
