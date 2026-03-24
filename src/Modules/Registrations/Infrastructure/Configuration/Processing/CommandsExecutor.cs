using CompanyName.MyMeetings.BuildingBlocks.Infrastructure.Configuration;
using CompanyName.MyMeetings.Modules.Registrations.Application.Contracts;

namespace CompanyName.MyMeetings.Modules.Registrations.Infrastructure.Configuration.Processing
{
    internal static class CommandsExecutor
    {
        private static readonly BaseCommandsExecutor _inner = new(RegistrationsCompositionRoot.Instance);

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
