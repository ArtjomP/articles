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
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null));
    }

    [Fact]
    public void ExecuteWithParameter_CommandNull_ThrowsArgumentNullExecption()
    {
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, new Object()));
    }

    [Fact]
    public void ExecuteWithParameterOfT_CommandNull_ThrowsArgumentNullExecption()
    {
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, Int32.MinValue));
    }

    [Fact]
    public void CanExecute_CommandNull_ThrowsArgumentNullExecption()
    {
        Assert.Throws<ArgumentNullException>(
            () => Target.CanExecute(null));
    }

    [Fact]
    public void CanExecuteWithParameter_CommandNull_ThrowsArgumentNullExecption()
    {
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, new Object()));
    }

    [Fact]
    public void CanExecuteWithParameterOfT_CommandNull_ThrowsArgumentNullExecption()
    {
        Assert.Throws<ArgumentNullException>(
            () => Target.Execute(null, Int32.MinValue));
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
        Action<Object?> action = o => actualParameter = o;
        var command = new ActionCommand(action);

        Target.Execute(command, expectedParameter);

        Assert.Equal(expectedParameter, actualParameter);
    }

    [Fact]
    public void Execute_ActionCommandWithTParam_PassesTheObjectToAction()
    {
        var expectedParameter = Int32.MinValue;
        Object? actualParameter = null;
        Action<Object?> action = o => actualParameter = o;
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
        Predicate<Object?> canExecute = o =>
        {
            actualParameter = o;
            return true;
        };
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
        Predicate<Object?> canExecute = o =>
        {
            actualParameter = o;
            return true;
        };
        var command = new ActionCommand(
           _ => { },
           canExecute);

        Target.CanExecute(command, expectedParameter);

        Assert.Equal(expectedParameter, actualParameter);
    }
}