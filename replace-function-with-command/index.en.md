# Replace Fuction with Command in C# using ICommand

This article looks for a way to reduce the complexity of a method. One of the common refactorings for this is [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring. The acticle shows a testable implementation of it using C# & it's ICommand interface in detail.

To reduce the complexity of a method we need to solve the next tasks:
- Define the method complexity & it's code smells.
- Describe known patterns to reduce the complexity.
- Create the testable implementation of the [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring using the ICommand interface to reduce the method complexity.
- Define the possible use cases for the Replace Function with Command refactoring for the C# stack.

## The method complexity & it's code smells

The [SOLID](https://en.wikipedia.org/wiki/SOLID) principle says that every class should only one responsibility. And what about the methods? Sometimes (very frequently actually) a method needs to have more than one responsibility. And the method gets very entangled & chained with all of its responsibilities. It's getting hard to change not only the method but the whole class. The code smells of it are:
- Method calls more than one private or public method of the class.
- More than one unit test fails when the method gets changed.
- Method's unit tests has more than one Assert.
- Many unit tests use a private method to set the state in the unit testing class.

For example class Foo needs to process (or send) some data only when the result is correct:
```cs
class Foo
{
    private IDataProcessor Processor { get; }

    public Foo(IDataProcessor processor)
    {
        Processor = processor;
    }
    
    public Int32 A { get; set; }

    public Int32 B { get; set; }

    public Int32 Result { get; set; }

    public Boolean ResultIsCorrect { get; set; }

    private void CalculateResult()
    {
        Result = A + B;
    }

    private Boolean CheckResultIsEven()
    {
        return Result % 2 == 0;
    }

    private void ValidateResult()
    {
        ResultIsCorrect = CheckResultIsEven();
    }

    public void SendData()
    {
        CalculateResult();
        ValidateResult();

        if (ResultIsCorrect)
        {
            Processor.Process(Result);
        }
    }
}
```

