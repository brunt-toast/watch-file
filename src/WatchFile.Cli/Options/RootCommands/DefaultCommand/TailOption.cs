using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.WatchFile.Cli.Options.RootCommands.DefaultCommand;

internal class TailOption : Option<int>
{
    private static int ValueFactory(ArgumentResult arg) => arg.Implicit ? int.MaxValue : 10;

    public TailOption() : base("--tail", "-t")
    {
        Description = "Output only the last n lines.";
        DefaultValueFactory = ValueFactory;
    }
}