#nullable enable

using System.Windows.Input;

namespace pt.CommandExecutor.Common;

public class CommandExecutor : ICommandExecutor
{
    public void Execute(ICommand command)
    {
        _ = command ?? 
            throw new ArgumentNullException(nameof(command));

        command.Execute(null);
    }

    public Boolean CanExecute(ICommand command)
    {
        _ = command ??
           throw new ArgumentNullException(nameof(command));

        return command.CanExecute(null);
    }

    public void Execute(
        ICommand command, 
        Object? commandParameter)
    {
        _ = command ??
            throw new ArgumentNullException(nameof(command));

        command.Execute(commandParameter);
    }

    public Boolean CanExecute(
        ICommand command, 
        Object? commandParameter)
    {
        _ = command ??
            throw new ArgumentNullException(nameof(command));

        return command.CanExecute(commandParameter);
    }

    public void Execute<T>(
        ICommand command,
        T commandParameter)
    {
        _ = command ??
            throw new ArgumentNullException(nameof(command));

        command.Execute(commandParameter);
    }

    public Boolean CanExecute<T>(
        ICommand command,
        T commandParameter)
    {
        _ = command ??
            throw new ArgumentNullException(nameof(command));

        return command.CanExecute(commandParameter);
    }
}