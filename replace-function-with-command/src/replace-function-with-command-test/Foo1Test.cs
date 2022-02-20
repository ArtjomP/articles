#nullable enable

using Moq;
using replace_function_with_command;
using System;
using Xunit;

namespace replace_function_with_command_test;

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

    private void PrepareToProcessTheData(Int32 a, Int32 b)
    {
        Target.A = a;
        Target.B = b;
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
        PrepareToProcessTheData(a, b);

        Target.ProcessData();

        DataProcessor.Verify(
            o => o.Process(Target.Result),
            Times.Exactly(invokeTimes));
    }
}
