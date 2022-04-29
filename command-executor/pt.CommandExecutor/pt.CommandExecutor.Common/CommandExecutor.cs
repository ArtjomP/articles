#nullable enable

using System.Windows.Input;
using Microsoft.Toolkit.Diagnostics;

namespace pt.CommandExecutor.Common;

public class CommandExecutor : ICommandExecutor
{
    public void Execute(ICommand command)
    {
        Guard.IsNotNull(command, nameof(command));

        command.Execute(null);
    }

    public Boolean CanExecute(ICommand command)
    {
        Guard.IsNotNull(command, nameof(command));

        return command.CanExecute(null);
    }

    public void Execute(
        ICommand command, 
        Object? commandParameter)
    {
        Guard.IsNotNull(command, nameof(command));

        command.Execute(commandParameter);
    }

    public Boolean CanExecute(
        ICommand command, 
        Object? commandParameter)
    {
        Guard.IsNotNull(command, nameof(command));

        return command.CanExecute(commandParameter);
    }

    public void Execute<T>(
        ICommand command,
        T commandParameter)
    {
        Guard.IsNotNull(command, nameof(command));

        command.Execute(commandParameter);
    }

    public Boolean CanExecute<T>(
        ICommand command,
        T commandParameter)
    {
        Guard.IsNotNull(command, nameof(command));

        return command.CanExecute(commandParameter);
    }
}