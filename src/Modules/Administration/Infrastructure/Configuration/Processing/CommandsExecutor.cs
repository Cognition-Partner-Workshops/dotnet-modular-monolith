using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration;
using CompanyName.MyMeetings.Modules.Administration.Application.Contracts;

namespace CompanyName.MyMeetings.Modules.Administration.Infrastructure.Configuration.Processing
{
    internal static class CommandsExecutor
    {
        private static readonly BaseCommandsExecutor _inner = new(AdministrationCompositionRoot.Instance);

        internal static async Task Execute(ICommand command)
        {
            await _inner.Execute(command);
        }

        internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
        {
            return await _inner.Execute(command);
        }
    }
}
