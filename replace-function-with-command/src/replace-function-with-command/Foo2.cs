namespace replace_function_with_command;

public class Foo2
{
    private IDataProcessor Processor { get; }

    private IResultCalculator ResultCalculator { get; }

    private IResultValidator Validator { get; }

    public Foo2(
        IDataProcessor processor,
        IResultCalculator resultCalculator,
        IResultValidator validator)
    {
        Processor = processor;
        ResultCalculator = resultCalculator;
        Validator = validator;
    }

    public Int32 A { get; set; }

    public Int32 B { get; set; }

    public Int32 Result { get; set; }

    public Boolean ResultIsCorrect { get; set; }

    public void ProcessData()
    {
        ResultCalculator.Calculate(this);
        Validator.Validate(this);

        if (ResultIsCorrect)
        {
            Processor.Process(Result);
        }
    }
}