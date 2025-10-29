namespace Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;

internal class NoAnsiFlag : Flag
{
    public NoAnsiFlag() : base("--no-ansi")
    {
        Description = "Don't apply ANSI colouring to diffed output.";
    }
}