# Replace Function with Command in C# using ICommand

> [Everything should be made as simple as possible, but not simpler. Albert Einstein](https://quoteinvestigator.com/2011/05/13/einstein-simple/).

This article looks for a way to reduce the complexity of a method. One of the common refactorings for this is [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring. The acticle shows a testable implementation of it using C# & it's ICommand interface in detail.

To reduce the complexity of a method the next tasks need to be solved:
- Define the method complexity & it's code smells.
- Describe known ways to reduce the method complexity.
- Create a testable implementation of the [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring using the ICommand interface to reduce the method complexity.
- Define the possible use cases for the [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring for the C# stack.

## The method complexity & it's code smells

The [SOLID](https://en.wikipedia.org/wiki/SOLID) principle says that every class should have only one responsibility. And what about the methods? Sometimes (very frequently actually) a method needs to have more than one responsibility. And the method gets very entangled & chained with all of its responsibilities. It's getting hard to change not only the method but the whole class. The code smells of it are:
- Method calls more than one private or public method of the class.
- More than one unit test fails when the method gets changed.
- Method's unit tests has more than one Assert.
- Unit tests use a private method to set the state in the unit testing class.

For example [class Foo](src/replace-function-with-command/Foo.cs) needs to process (or send) some data only when the result is correct. This artical deals with the evolution of the **Foo** class by adding generation number to its name, for example, **Foo0**, **Foo1**... Initial **Foo0** looks like:

```cs
public class Foo0
{
    private IDataProcessor Processor { get; }

    public Foo0(IDataProcessor processor)
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
```

[Unit test](src/replace-function-with-command-test/FooTest.cs) for the ProcessData method looks like this (using [XUnit](https://xunit.net/) test framework):
```cs
public class Foo0Test
{
    private Mock<IDataProcessor> DataProcessor { get; }

    private Foo0 Target { get; }

    public Foo0Test()
    {
        DataProcessor = new Mock<IDataProcessor>();

        Target = new Foo0(DataProcessor.Object);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 1, 1)]
    public void ProcessData_Data_CallsProcessOnlyForEvenResult(
        Int32 a,
        Int32 b,
        Int32 invokeTimes)
    {
        Target.A = a;
        Target.B = b;

        Target.ProcessData();

        DataProcessor.Verify(
            o => o.Process(Target.Result),
            Times.Exactly(invokeTimes));
    }
}
```

What does the unit test test here? It tests particular data cases for Foo & DataProcessor. And what are the resposibilites of the Foo class? They are (and even private method names help to define them):
1. Calculate the Result.
2. Validate the Result.
3. Call DataProcessor->Process Result only when the ResultIsCorrect is true.

All the responsibilites need to be separetaed from each other & should be testable on its own.

## Known ways to solve the method complexity

To separate the **Foo** class responsibilities & reduce the **ProcessData** complexity the next ways can be used:

0. Do nothing.  
1. Add more unit tests for testing all the class responsibilities and bind the unit tests to each other.
2. Use the [Fundamental theorem of software engineering](https://en.wikipedia.org/wiki/Fundamental_theorem_of_software_engineering) and add more abstractions.
3. Replace Function with ICommand.

### 0. Do nothing

The test case does not cover the initial goals (responsibilities) of the class but it's hard to imagine for this particular case what can go wrong.

In the more complex cases when the test gets more entangled (uses more parameters or uses private methods to set up the class state) it's getting hard to do nothing & stay with the current way & its test.

### 1. Add more unit tests for testing all the class responsibilities and bind the unit tests to each other.

This way can help in some very simple cases sometimes but it's not expandable & make things even worse. Actually, this is one of the code smells you can find at the beginning of the article.
To unit test the responsibiliites the **Foo** class needs to open its private methods:

```cs
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
```

The example of such test case is here:
```cs
public class Foo1Test
{
    private Mock<IDataProcessor> DataProcessor { get; }

    private Foo1 Target { get; }

    public Foo1Test()
    {
        DataProcessor = new Mock<IDataProcessor>();

        Target = new Foo1(DataProcessor.Object);
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 1, 2)]
    public void CalculateResult_Data_CheckResult(
        Int32 a,
        Int32 b,
        Int32 result)
    {
        Target.A = a;
        Target.B = b;

        Target.CalculateResult();

        Assert.Equal(result, Target.Result);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public void ValidateResult_Data_CheckResultIsCorrect(
        Int32 result,
        Boolean resultIsCorrect)
    {
        Target.Result = result;

        Target.ValidateResult();

        Assert.Equal(resultIsCorrect, Target.ResultIsCorrect);
    }

    private void PrepareToProcessTheData()
    {
        Target.CalculateResult();
        Target.ValidateResult();
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 1, 1)]
    public void ProcessData_Data_CallsProcessOnlyForEvenResult(
        Int32 a,
        Int32 b,
        Int32 invokeTimes)
    {
        Target.A = a;
        Target.B = b;
        PrepareToProcessTheData();

        Target.ProcessData();

        DataProcessor.Verify(
            o => o.Process(Target.Result),
            Times.Exactly(invokeTimes));
    }
}
```

These tests help to understand & test the initial intentions (goals, responsibilities) of the class. It initiates the further possible refactorings such as remove invoking of the **CalculateResult** & **ValidateResult** from the **ProcessData** method but it opens more questions such as:
- How to test that method **CalculateResult** & **ValidateResult** get invoked before **ProcessData**?
- How to test the order of call?

> **The key issue here is that it's hard to test that particular method was called.**

This way looks simple at the first sight but its complexity grows exponentially with adding responsibilities and logic to the class. Use it to find the code smell.

It is mentioned here as far as it's simplicity can be helpful & complexity can be handled with the **ICommand** interface. Look at pattern #3.

### 2. Use the [Fundamental theorem of software engineering](https://en.wikipedia.org/wiki/Fundamental_theorem_of_software_engineering) and add more abstractions.

Some abstractions such as **IResultCalculator** & **IResultValidator** can be added here. This abstractions can be injected as [Dependency injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) to the Foo class via the constructor or as the ProcessData method parameters. Actually part of this pattern has been implemented here with the **IDataProcessor** injection. But this principle violates another principle *do not create too much abstractions*.

The **Foo** class looks this way:
```cs
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
```

The unit tests for it look like this:

```cs
public class Foo2Test
{
    private Mock<IDataProcessor> DataProcessor { get; }

    private Mock<IResultCalculator> ResultCalculator { get; }

    private Mock<IResultValidator> Validator { get; }

    private Foo2 Target { get; }

    public Foo2Test()
    {
        DataProcessor = new Mock<IDataProcessor>();
        ResultCalculator = new Mock<IResultCalculator>();
        Validator = new Mock<IResultValidator>();

        Target = new Foo2(
            DataProcessor.Object,
            ResultCalculator.Object,
            Validator.Object);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void ProcessData_ResultIsCorrect_CallsProcessOnlyForCorrectResult(
        Boolean resultIsCorrect,
        Int32 invokeTimes)
    {
        var result = 1111;
        Target.Result = result;
        Target.ResultIsCorrect = resultIsCorrect;

        Target.ProcessData();

        DataProcessor.Verify(
            o => o.Process(result),
            Times.Exactly(invokeTimes));
    }

    [Fact]
    public void ProcessData_CallsCalculateFromResultCalculator()
    {
        Target.ProcessData();

        ResultCalculator.Verify(o => o.Calculate(Target));
    }

    [Fact]
    public void ProcessData_CallsValidateFromValidator()
    {
        Target.ProcessData();

        Validator.Verify(o => o.Validate(Target));
    }
}
```

Now all the responsibilities are separated & the intents can be tested without actual data. It starts some more refactorings like moving **IResultCalculator** to the **IResultValidator** and call only **Validate** from the **ProcessData** method, etc. 

This is a good way for solving the method complexity. It's testable & expandable. But if the created abstractions (**IResultCalculator**, **IResultValidator**) used only in one class it requires too many code. The abstraction implementations should be tested also. For example for one **Foo** class 6 more code files required: **IResultCalculator**, **ResultCalculatorImplementation**, **ResultCalculatorImplementationTest**, **IResultValidator**, **ResultValidatorImplementation**, **ResultValidatorImplementationTest**.

### 3. Replace Function with ICommand.

This refactoring allows keeping the simplicity of method #1. It needs some changes to the code:
- Wrap the private methods (responsiblities) with commands & make them public. 

The **Foo** class looks like this (the **ICommand** implementation used here is simple, use one from well-known libraries such as [Prism](https://github.com/PrismLibrary/Prism/blob/342689ba9a10e511418943438d0f5c57534ca623/src/Prism.Core/Commands/DelegateCommand.cs), [ReactiveUI](https://github.com/reactiveui/ReactiveUI/blob/a941e556a6bd34a6f631354da800f503661b5d48/src/ReactiveUI/ReactiveCommand/ReactiveCommand.cs), etc):

```cs
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
```

The unit test looks like this now:

```cs
public class Foo3Test
{
    private Mock<IDataProcessor> DataProcessor { get; }

    private Foo3 Target { get; }

    public Foo3Test()
    {
        DataProcessor = new Mock<IDataProcessor>();

        Target = new Foo3(DataProcessor.Object);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 1, 1)]
    public void ProcessData_Data_CallsProcessOnlyForEvenResult(
       Int32 a,
       Int32 b,
       Int32 invokeTimes)
    {
        Target.A = a;
        Target.B = b;

        Target.ProcessData();

        DataProcessor.Verify(
            o => o.Process(Target.Result),
            Times.Exactly(invokeTimes));
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 1, 2)]
    public void CalculateResultCommand_Data_CheckResult(
       Int32 a,
       Int32 b,
       Int32 result)
    {
        Target.A = a;
        Target.B = b;

        Target.CalculateResultCommand.Execute(null);

        Assert.Equal(result, Target.Result);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public void ValidateResultCommand_Data_CheckResultIsCorrect(
        Int32 result,
        Boolean resultIsCorrect)
    {
        Target.Result = result;

        Target.ValidateResultCommand.Execute(null);

        Assert.Equal(resultIsCorrect, Target.ResultIsCorrect);
    }
}
```

The unit test looks pretty similar as for the way #1. And it's hard to see any advantages here in comparison with the way #1. Do you remember the key issue of way #1? **It's hard to test that particular method was called**.

Adding only one reusable abstraction can make this way testable & expandable.

## Create the testable implementation of the [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html)

The only required abstraction **ICommandExecutor** for making the way #3 testable & expandable looks like this:

```cs
public interface ICommandExecutor
{
    void Execute(ICommand command);
    void Execute(ICommand command, Object? parameter);
}
```

The **Foo** class using this abstraction looks like this:

```cs
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
```

And the unit test looks like this:

```cs
public class Foo4Test
{
    private Mock<IDataProcessor> DataProcessor { get; }
    private Mock<ICommandExecutor> CommandExecutor { get; }

    private Foo4 Target { get; }

    public Foo4Test()
    {
        DataProcessor = new Mock<IDataProcessor>();
        CommandExecutor = new Mock<ICommandExecutor>();

        Target = new Foo4(
            DataProcessor.Object,
            CommandExecutor.Object);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void ProcessData_ResultIsCorrect_CallsProcessOnlyForCorrectResult(
        Boolean resultIsCorrect,
        Int32 invokeTimes)
    {
        var result = 1111;
        Target.Result = result;
        Target.ResultIsCorrect = resultIsCorrect;

        Target.ProcessData();

        DataProcessor.Verify(
            o => o.Process(result),
            Times.Exactly(invokeTimes));
    }


    [Fact]
    public void ProcessData_ExecutesCalculateResultCommand()
    {
        Target.ProcessData();

        CommandExecutor.Verify(o => o.Execute(Target.CalculateResultCommand));
    }

    [Fact]
    public void ProcessData_ExecutesValidateResultCommand()
    {
        Target.ProcessData();

        CommandExecutor.Verify(o => o.Execute(Target.ValidateResultCommand));
    }
}
```

If we remove **CommandExecutor.Execute(ValidateResultCommand);** from the **ProcessData** method only one test would fail. 

[Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring joined with the [Fundamental theorem of software engineering](https://en.wikipedia.org/wiki/Fundamental_theorem_of_software_engineering) allowed to solve method complexity in a testable & expandable way. And it doesn't require creating abstractions only for the tested class (**Foo**). The created abstraction **ICommandExecutor** will be reused many times within the program.

## Define the possible use cases for the [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring for the C# stack

The possible use cases are:
- Changing state of the ViewModel in the [Model-View-ViewModel Pattern](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm).
- Method refactoring to reduce it's complexity.
- Method refactoring to remove private method dependencies.
- With the (Chain-of-responsibility pattern).

The [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) is pretty common it allows to make the code testable & expandable. 
Following the principle that everything should be as simple as possible, but not simpler the next algorthm can be proposed to use for solving the method complexity.

```mermaid
flowchart TD
    A[Refactor the complex method] --> B{The method is trivial and can be tested with trivial data?};
    B -- Yes --> C[Use way #0 or #1 (Do nothing or simple data driven tests)];
```

## Conclusion

The article solved the next tasks:
- Defined the method complexity & it's code smells.
- Described known ways to reduce the methods complexity.
- Created the testable implementation of the [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring using the ICommand interface to reduce the method complexity.
- Defined the possible use cases for the [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring for the C# stack.

Also, an algorithm for refactoring a complex method has been proposed.

The key abstraction [ICommandExecutor](replace-function-with-command\src\replace-function-with-command\ICommandExecutor.cs) that allows to make [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring testable & expandable has been proposed.

Using ICommand with [Model-View-ViewModel Pattern](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm) is handful and allows to test & maintain ViewModels of any complexity.

The **Func** and **Action** can be used also to solve the method complexity in the testable & expandable way. Expanding the **ICommandExecutor** to support **Task** & different **ICommand** implemenations will be proposed in the future articles.

All the souce code can be found in the [src](src) folder.