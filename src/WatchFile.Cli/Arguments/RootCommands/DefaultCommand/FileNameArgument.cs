using System.CommandLine;
using System.CommandLine.Parsing;

namespace Dev.JoshBrunton.WatchFile.Cli.Arguments.RootCommands.DefaultCommand;

internal class FileNameArgument : Argument<string>
{
    public FileNameArgument() : base("filename")
    {
        Description = "The name of the file to use";
        Validators.Add(FileExistsValidation);
        Arity = ArgumentArity.ExactlyOne;
    }

    private void FileExistsValidation(ArgumentResult obj)
    {
        string? path = obj.GetValue(this);
        if (!File.Exists(path))
        {
            obj.AddError($"The file \"{path}\" does not exist.");
        }
    }
}
