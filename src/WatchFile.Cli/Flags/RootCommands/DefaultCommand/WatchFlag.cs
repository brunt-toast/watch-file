namespace Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;

internal class WatchFlag : Flag
{
    public WatchFlag() : base("--watch", "-w")
    {
        Description = "Instead of reading at a fixed interval, ask the file system to watch for changes. Not all file systems support this option.";
    }
}