#nullable enable

using System.Windows.Input;

namespace replace_function_with_command;

public class Foo3
{
    private IDataProcessor Processor { get; }
    
    public Foo3(IDataProcessor processor)
    {
        Processor = processor;

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
        CalculateResultCommand.Execute(null);
        ValidateResultCommand.Execute(null);

        if (ResultIsCorrect)
        {
            Processor.Process(Result);
        }
    }
}