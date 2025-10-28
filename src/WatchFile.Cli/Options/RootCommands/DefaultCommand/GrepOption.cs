using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.WatchFile.Cli.Options.RootCommands.DefaultCommand;

internal class GrepOption : Option<string>
{
    private static string ValueFactory(ArgumentResult _) => ".*";

    public GrepOption() : base("--grep", "-g")
    {
        Description = "Only print lines matching the given regular expression.";
        DefaultValueFactory = ValueFactory;
    }
}