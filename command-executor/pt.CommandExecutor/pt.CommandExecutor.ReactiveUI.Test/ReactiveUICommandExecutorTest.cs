#nullable enable

using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using pt.CommandExecutor.Common;
using ReactiveUI;
using Xunit;

namespace pt.CommandExecutor.ReactiveUI.Test;

public class ReactiveUICommandExecutorTest
{
    private ReactiveUICommandExecutor Target { get; } = new ();

    [Fact]
    public void ExecuteAsync_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(
            () => Target.ExecuteAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void ExecuteAsyncGeneric_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(
            () => Target.ExecuteAsync<Object, Object>(null, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void ExecuteAsyncGenericUnit_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(
            () => Target.ExecuteAsync<Object>(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public void ExecuteDefaultAsync_CommandNull_ThrowsArgumentNullExecption()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(
            () => Target.ExecuteDefaultAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    [Fact]
    public async Task ExecuteAsync_Command_ExecutesCommand()
    {
        var executed = false;
        var command = ReactiveCommand
            .Create(() => { executed = true; });

        await Target
            .ExecuteAsync(command)
            .ConfigureAwait(false);

        Assert.True(executed);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ExecuteGenericAsync_Command_ExecutesCommand(
        Boolean expected)
    {
        var command = ReactiveCommand
            .Create(() => expected);

        var actual = await Target
            .ExecuteAsync(command, Unit.Default)
            .ConfigureAwait(false);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ExecuteGenericUnitAsync_Command_ExecutesCommand(
        Boolean expected)
    {
        var command = ReactiveCommand
            .Create(() => expected);

        var actual = await Target
            .ExecuteAsync(command)
            .ConfigureAwait(false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ExecuteDefaultAsync_ReactiveCommand_ExecutesCommand()
    {
        var executed = false;
        var command = ReactiveCommand
            .Create(() => { executed = true; });

        await Target
            .ExecuteDefaultAsync(command)
            .ConfigureAwait(false);

        Assert.True(executed);
    }

    [Fact]
    public async Task ExecuteDefaultAsync_ActionCommand_ExecutesCommand()
    {
        var executed = false;
        var command = new ActionCommand(_ => { executed = true; });

        await Target
            .ExecuteDefaultAsync(command)
            .ConfigureAwait(false);

        Assert.True(executed);
    }
}