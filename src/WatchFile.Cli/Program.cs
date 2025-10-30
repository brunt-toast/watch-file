using Dev.JoshBrunton.WatchFile.Cli.Commands;
using Dev.JoshBrunton.WatchFile.Cli.Commands.RootCommands;

var rootCommand = new DefaultCommand { new CompletionsCommand() };
return rootCommand.Execute(args);