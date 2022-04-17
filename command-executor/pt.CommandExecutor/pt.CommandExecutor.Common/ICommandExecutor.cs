#nullable enable

using System;
using System.Windows.Input;

namespace pt.CommandExecutor;

public interface ICommandExecutor
{
    void Execute(ICommand command);

    Boolean CanExecute(ICommand command);

    void Execute(ICommand command, Object? commandParameter);

    Boolean CanExecute(ICommand command, Object? commandParameter);

    void Execute<T>(ICommand command, T commandParameter);

    Boolean CanExecute<T>(ICommand command, T commandParameter);
}