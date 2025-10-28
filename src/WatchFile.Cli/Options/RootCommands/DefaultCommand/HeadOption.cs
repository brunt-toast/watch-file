using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.WatchFile.Cli.Options.RootCommands.DefaultCommand;

internal class HeadOption : Option<int>
{
    private static int ValueFactory(ArgumentResult arg) => arg.Implicit ? int.MaxValue : 10;

    public HeadOption() : base("--head", "-h")
    {
        Description = "Output only the first n lines.";
        DefaultValueFactory = ValueFactory;
    }
}