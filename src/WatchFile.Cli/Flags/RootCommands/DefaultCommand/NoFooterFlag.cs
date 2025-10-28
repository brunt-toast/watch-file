namespace Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;

internal class NoFooterFlag : Flag
{
    public NoFooterFlag() : base("--no-footer")
    {
        Description = "Skip outputting the footer (which shows the filename and time of read)";
    }
}