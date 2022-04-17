using System.Windows.Input;

namespace pt.CommandExecutor.Common;

public class ActionCommand : ICommand
{
    private readonly Predicate<Object?> _canExecute;
    private readonly Action<Object?> _execute;

    public ActionCommand(
        Action<Object?> execute)
        : this(
             execute,
             o => true)
    {
    }

    public ActionCommand(
        Action<Object?> execute,
        Predicate<Object?> canExecute)
    {
        _canExecute = canExecute ??
            throw new ArgumentNullException(nameof(canExecute));
        _execute = execute ??
            throw new ArgumentNullException(nameof(execute));
    }

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanxecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }

    public Boolean CanExecute(Object? parameter)
    {
        var result = true;
        var canExecute = _canExecute;
        if (canExecute != null)
        {
            result = canExecute.Invoke(parameter);
        }

        return result;
    }

    public void Execute(Object? parameter)
    {
        _execute(parameter);
    }
}