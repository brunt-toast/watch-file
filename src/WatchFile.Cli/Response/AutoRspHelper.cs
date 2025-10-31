using System.CommandLine;

namespace Dev.JoshBrunton.WatchFile.Cli.Response;

internal class AutoRspHelper<T> where T : Command
{
    private readonly T _command;

    public AutoRspHelper(T command)
    {
        _command = command;
    }

    private string GetAutoRspPath()
    {
        string fileName = typeof(T).IsAssignableTo(typeof(RootCommand)) 
            ? "watch-file.rsp" 
            : $"watch-file.{_command.Name}.rsp";

        return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "watch-file", fileName);
    }

    public void MutateArgs(ref string[] args)
    {
        string autoRspFilePath = GetAutoRspPath();
        if (!File.Exists(autoRspFilePath))
        {
            return;
        }

        string content;
        try
        {
            content = File.ReadAllText(autoRspFilePath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load auto-response from \"{autoRspFilePath}\" - error {ex}");
            return;
        }

        args = [.. System.CommandLine.Parsing.CommandLineParser.SplitCommandLine(content), .. args];
    }
}
