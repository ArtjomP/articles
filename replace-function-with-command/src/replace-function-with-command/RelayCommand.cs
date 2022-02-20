using System.Windows.Input;

namespace replace_function_with_command;

public class RelayCommand : ICommand
{
    private readonly Action _execute;

    public RelayCommand(Action execute)
    {
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged;

    public Boolean CanExecute(Object? parameter)
    {
        return true;
    }

    public void Execute(Object? parameter)
    {
        _execute();
    }
}