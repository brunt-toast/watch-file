using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.WatchFile.Cli.Options.RootCommands.DefaultCommand;

internal class IntervalOption : Option<int>
{
    private static int ValueFactory(ArgumentResult _) => 1_000;

    public IntervalOption() : base("--interval", "-i")
    {
        Description = "Wait n milliseconds between file accesses.";
        DefaultValueFactory = ValueFactory;
    }
}