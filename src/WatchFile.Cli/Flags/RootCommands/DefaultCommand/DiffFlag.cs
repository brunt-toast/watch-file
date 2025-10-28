namespace Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;

internal class DiffFlag : Flag
{
    public DiffFlag() : base("--diff","-d")
    {
        Description = "Each time the file is changed, generate a patch vs the previous known version";
    }
}