namespace Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;
internal class ClearFlag : Flag
{
    public ClearFlag() : base("--clear", "-c")
    {
        Description = "Clear the console before new output.";
    }
}