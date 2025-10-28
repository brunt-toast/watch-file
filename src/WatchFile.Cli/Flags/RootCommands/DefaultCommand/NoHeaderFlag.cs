namespace Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;

internal class NoHeaderFlag : Flag
{
    public NoHeaderFlag() : base("--no-header")
    {
        Description = "Skip outputting the header (which shows the filename and time of read)";
    }
}