#nullable enable

using Moq;
using replace_function_with_command;
using System;
using Xunit;

namespace replace_function_with_command_test;

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