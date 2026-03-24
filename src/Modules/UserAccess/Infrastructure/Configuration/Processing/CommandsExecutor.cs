using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration;
using CompanyName.MyMeetings.Modules.UserAccess.Application.Contracts;

namespace CompanyName.MyMeetings.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    internal static class CommandsExecutor
    {
        private static readonly BaseCommandsExecutor _inner = new(UserAccessCompositionRoot.Instance);

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
