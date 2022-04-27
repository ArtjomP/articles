#nullable enable

using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Diagnostics;
using ReactiveUI;

namespace pt.CommandExecutor.ReactiveUI;

public class ReactiveUICommandExecutor
    : Common.CommandExecutor
    , IReactiveUICommandExecutor
{
    public async Task ExecuteAsync(
        ReactiveCommand<Unit, Unit> command)
    {
        Guard.IsNotNull(command, nameof(command));

        await command.Execute();
    }

    public async Task<TResult> ExecuteAsync<TParam, TResult>(
        ReactiveCommand<TParam, TResult> command,
        TParam parameter)
    {
        Guard.IsNotNull(command, nameof(command));

        var result = await command.Execute(parameter);

        return result;
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        ReactiveCommand<Unit, TResult> command)
    {
        Guard.IsNotNull(command, nameof(command));

        var result = await command.Execute(Unit.Default);

        return result;
    }

    public async Task ExecuteDefaultAsync(ICommand command)
    {
        Guard.IsNotNull(command, nameof(command));

        if (command is ReactiveCommand<Unit, Unit> reactiveCommand)
        {
            await reactiveCommand.Execute();
        }
        else
        {
            command.Execute(Unit.Default);
        }
    }
}