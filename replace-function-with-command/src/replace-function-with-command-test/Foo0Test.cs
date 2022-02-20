#nullable enable

using Moq;
using replace_function_with_command;
using System;
using Xunit;

namespace replace_function_with_command_test;

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