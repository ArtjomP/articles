#nullable enable

using Moq;
using replace_function_with_command;
using System;
using Xunit;

namespace replace_function_with_command_test;

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