using System.CommandLine;
using System.CommandLine.Parsing;
using Dev.JoshBrunton.WatchFile.Cli.Enums;

namespace Dev.JoshBrunton.WatchFile.Cli.Options.CompletionsCommand;

internal class ShellOption : Option<Shell>
{
    private static Shell ValueFactory(ArgumentResult _) => (Shell)0;
    public ShellOption() : base("--shell", "-s")
    {
        Description = "Specify the shell";
        Arity = ArgumentArity.ExactlyOne;
        DefaultValueFactory = ValueFactory;
    }
}