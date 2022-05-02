#nullable enable

using System.Reactive;
using System.Windows.Input;
using ReactiveUI;

namespace pt.CommandExecutor.ReactiveUI;

public interface ICommandExecutorReactiveUI
    : ICommandExecutor
{
    Task ExecuteAsync(
        ReactiveCommand<Unit, Unit> command);

    Task<TResult> ExecuteAsync<TParam, TResult>(
        ReactiveCommand<TParam, TResult> command,
        TParam parameter);

    Task<TResult> ExecuteAsync<TResult>(
        ReactiveCommand<Unit, TResult> command);

    Task ExecuteDefaultAsync(ICommand command);
}