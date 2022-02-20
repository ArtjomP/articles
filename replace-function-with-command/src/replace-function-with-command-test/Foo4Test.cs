#nullable enable

using Moq;
using replace_function_with_command;
using System;
using Xunit;

namespace replace_function_with_command_test;

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