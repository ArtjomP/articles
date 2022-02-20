namespace replace_function_with_command;

public class Foo1
{
    private IDataProcessor Processor { get; }

    public Foo1(IDataProcessor processor)
    {
        Processor = processor;
    }

    public Int32 A { get; set; }

    public Int32 B { get; set; }

    public Int32 Result { get; set; }

    public Boolean ResultIsCorrect { get; set; }

    //private -> public
    public void CalculateResult()
    {
        Result = A + B;
    }

    private Boolean CheckResultIsEven()
    {
        return Result % 2 == 0;
    }

    //private -> public
    public void ValidateResult()
    {
        ResultIsCorrect = CheckResultIsEven();
    }

    public void ProcessData()
    {
        CalculateResult();
        ValidateResult();

        if (ResultIsCorrect)
        {
            Processor.Process(Result);
        }
    }
}