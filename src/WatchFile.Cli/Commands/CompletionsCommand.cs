using System.CommandLine;
using System.Reflection;
using Dev.JoshBrunton.WatchFile.Cli.Consts;
using Dev.JoshBrunton.WatchFile.Cli.Enums;
using Dev.JoshBrunton.WatchFile.Cli.Options.CompletionsCommand;

namespace Dev.JoshBrunton.WatchFile.Cli.Commands;

internal class CompletionsCommand : Command
{
    private readonly ShellOption _shellOption = new();

    public CompletionsCommand() : base("completions", "Output a script to enable command-line completions for this tool.")
    {
        Options.Add(_shellOption);
        SetAction(ExecuteAction);
    }

    private int ExecuteAction(ParseResult parseResult)
    {
        Shell shell = parseResult.GetValue(_shellOption);

        string streamName = shell switch
        {
            Shell.Bash => "Dev.JoshBrunton.WatchFile.Cli.Resources.watch-file-completion.bash",
            _ => ""
        };

        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(streamName);
        if (stream is null)
        {
            Console.WriteLine("Couldn't open the manifest resource stream for the requested shell.");
            return ExitCodes.EmbeddedResourceStreamNotFound;
        }

        using StreamReader reader = new(stream);
        string content = reader.ReadToEnd();
        Console.WriteLine(content);

        return ExitCodes.Success;
    }
}
