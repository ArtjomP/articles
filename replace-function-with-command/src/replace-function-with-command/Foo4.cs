#nullable enable

using System.Windows.Input;

namespace replace_function_with_command;

public class Foo4
{
    private IDataProcessor Processor { get; }

    private ICommandExecutor CommandExecutor { get; }

    public Foo4(
        IDataProcessor processor,
        ICommandExecutor commandExecutor)
    {
        Processor = processor;
        CommandExecutor = commandExecutor;

        CalculateResultCommand = new RelayCommand(CalculateResult);
        ValidateResultCommand = new RelayCommand(ValidateResult);
    }

    public Int32 A { get; set; }

    public Int32 B { get; set; }

    public Int32 Result { get; set; }

    public Boolean ResultIsCorrect { get; set; }

    public ICommand CalculateResultCommand { get; }

    private void CalculateResult()
    {
        Result = A + B;
    }

    private Boolean CheckResultIsEven()
    {
        return Result % 2 == 0;
    }

    public ICommand ValidateResultCommand { get; }

    private void ValidateResult()
    {
        ResultIsCorrect = CheckResultIsEven();
    }

    public void ProcessData()
    {
        CommandExecutor.Execute(CalculateResultCommand);
        CommandExecutor.Execute(ValidateResultCommand);

        if (ResultIsCorrect)
        {
            Processor.Process(Result);
        }
    }
}