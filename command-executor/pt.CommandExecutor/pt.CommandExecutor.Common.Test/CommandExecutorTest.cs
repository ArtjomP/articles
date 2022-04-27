#nullable enable

using System;
using Xunit;

namespace pt.CommandExecutor.Common.Test;

public class CommandExecutorTest
{
    private CommandExecutor Target { get; } = new ();

    [Fact]
    public void Execute_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void ExecuteWithParameter_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, new Object()));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void ExecuteWithParameterOfT_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, Int32.MinValue));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void CanExecute_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(
            () => Target.CanExecute(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void CanExecuteWithParameter_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, new Object()));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void CanExecuteWithParameterOfT_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, Int32.MinValue));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void Execute_ActionCommandNoParameter_ExecutesAction()
    {
        var executed = false;
        var command = new ActionCommand(o => executed = true);

        Target.Execute(command);

        Assert.True(executed);
    }

    [Fact]
    public void Execute_ActionCommandWithObjectParam_PassesTheObjectToAction()
    {
        var expectedParameter = new Object();
        Object? actualParameter = null;
        void action(Object? o) => actualParameter = o;
        var command = new ActionCommand(action);

        Target.Execute(command, expectedParameter);

        Assert.Equal(expectedParameter, actualParameter);
    }

    [Fact]
    public void Execute_ActionCommandWithTParam_PassesTheObjectToAction()
    {
        var expectedParameter = Int32.MinValue;
        Object? actualParameter = null;
        void action(Object? o) => actualParameter = o;
        var command = new ActionCommand(action);

        Target.Execute(command, expectedParameter);

        Assert.Equal(expectedParameter, actualParameter);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CanExecute_ActionCommandNoParameter_ExecutesAction(
        Boolean expectedCanExecute)
    {
        var command = new ActionCommand(
            _ => { },
            _ => expectedCanExecute);

        var actual = Target.CanExecute(command);

        Assert.Equal(expectedCanExecute, actual);
    }

    [Fact]
    public void CanExecute_ActionCommandWithObjectParam_PassesTheObjectToAction()
    {
        var expectedParameter = new Object();
        Object? actualParameter = null;
        Boolean canExecute(Object? o)
        {
            actualParameter = o;
            return true;
        }
        var command = new ActionCommand(
           _ => { },
           canExecute);

        Target.CanExecute(command, expectedParameter);

        Assert.Equal(expectedParameter, actualParameter);
    }

    [Fact]
    public void CanExecute_ActionCommandWithTParam_PassesTheObjectToAction()
    {
        var expectedParameter = Int32.MinValue;
        Object? actualParameter = null;
        Boolean canExecute(Object? o)
        {
            actualParameter = o;
            return true;
        }
        var command = new ActionCommand(
           _ => { },
           canExecute);

        Target.CanExecute(command, expectedParameter);

        Assert.Equal(expectedParameter, actualParameter);
    }
}
