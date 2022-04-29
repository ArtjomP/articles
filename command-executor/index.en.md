# Executing ICommand in a testable way

[ICommand](https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.icommand?view=net-5.0) is the key interface used in C# for implementing [Command pattern](https://en.wikipedia.org/wiki/Command_pattern). This pattern can be helpful not only for UI frameworks but for [replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring. Using the replace Function with Command refactoring for solving method
complexity in a testable way described [here](/replace-function-with-command/index.en.md). The article introduced abstraction [ICommandExecutor](/replace-function-with-command/src/replace-function-with-command/ICommandExecutor.cs) that allows to make [Replace Function with Command](https://refactoring.com/catalog/replaceFunctionWithCommand.html) refactoring testable & expandable.

This article develops the initial proposal to the [ICommandExecutor](/command-executor/pt.CommandExecutor/pt.CommandExecutor.Common/ICommandExecutor.cs) makes two implementations of it [CommandExecutor](/command-executor/pt.CommandExecutor/pt.CommandExecutor.Common/CommandExecutor.cs) and [ReactiveUICommandExecutor](/command-executor/pt.CommandExecutor/pt.CommandExecutor.ReactiveUI/ReactiveUICommandExecutor.cs) for basic commands and
[ReactiveCommand](https://www.reactiveui.net/docs/handbook/commands/). All this implementations can be found in the corresponding Nuget packages:

[![Nuget](https://img.shields.io/nuget/v/pt.CommandExecutor.Common?label=pt.CommandExecutor.Common)](https://www.nuget.org/packages/pt.CommandExecutor.Common/)

[![Nuget](https://img.shields.io/nuget/v/pt.CommandExecutor.ReactiveUI?label=pt.CommandExecutor.ReactiveUI)](https://www.nuget.org/packages/pt.CommandExecutor.Common/)

[pt.CommandExecutor.ReactiveUI](/command-executor/pt.CommandExecutor/pt.CommandExecutor.ReactiveUI/) includes [pt.CommandExecutor.Common](/command-executor/pt.CommandExecutor/pt.CommandExecutor.Common/) as a reference.

Build pipeline:

[![Build Status](https://dev.azure.com/pteam/Public/_apis/build/status/pt.CommandExecutor?branchName=main)](https://dev.azure.com/pteam/Public/_build?definitionId=39)

## Quick start

`
Install-Package pt.CommandExecutor.Common
`

**In code**:
```cs
public class Foo
{
    public ICommand Command 
    {
        get; 
    } = new ActionCommand(() => Console.WriteLine("Hello world"));
    
    private ICommandExecutor CommandExecutor { get; }

    public Foo(ICommandExecutor commandExecutor)
    {
        CommandExecutor = commandExecutor;
    }

    public void Method()
    {
        commandExecutor.Execute(command);
    }
}
```

**Unit test**:
```cs
[Fact]
public void Method_Call_ExecutesCommand()
{
    var commandExector = new Mock<ICommandExecutor>();
    var target = new Foo(commandExecutor.Object);
    
    target.Method();

    commandExecutor.Verify(o => o.Execute(target.Command));
}
```

More details can be found in the [article](/replace-function-with-command/index.en.md#create-the-testable-implementation-of-the-replace-function-with-commandhttpsrefactoringcomcatalogreplacefunctionwithcommandhtml).