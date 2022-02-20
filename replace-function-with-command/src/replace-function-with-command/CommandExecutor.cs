#nullable enable

using System.Windows.Input;

namespace replace_function_with_command
{
    internal class CommandExecutor : ICommandExecutor
    {
        public void Execute(ICommand command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            command.Execute(null);
        }

        public void Execute(ICommand command, Object? parameter)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            command.Execute(parameter);
        }
    }
}