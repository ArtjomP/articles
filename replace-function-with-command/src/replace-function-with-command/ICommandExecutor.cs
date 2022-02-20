#nullable enable

using System.Windows.Input;

namespace replace_function_with_command;

public interface ICommandExecutor
{
    void Execute(ICommand command);
    void Execute(ICommand command, Object? parameter);
}
