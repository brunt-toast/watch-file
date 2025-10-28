using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.WatchFile.Cli.Flags;

internal abstract class Flag : Option<bool>
{
    private static bool ValueFactory(ArgumentResult _) => false;

    protected Flag(string name, params string[] aliases) : base(name, aliases)
    {
        DefaultValueFactory = ValueFactory;
    }
}
