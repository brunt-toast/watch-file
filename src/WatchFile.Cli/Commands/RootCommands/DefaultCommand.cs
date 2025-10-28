using Dev.JoshBrunton.WatchFile.Cli.Arguments.RootCommands.DefaultCommand;
using Dev.JoshBrunton.WatchFile.Cli.Consts;
using Dev.JoshBrunton.WatchFile.Cli.Extensions.System;
using Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;
using Dev.JoshBrunton.WatchFile.Cli.Options.RootCommands.DefaultCommand;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System.CommandLine;
using System.Text;
using System.Text.RegularExpressions;

namespace Dev.JoshBrunton.WatchFile.Cli.Commands.RootCommands;

internal class DefaultCommand : RootCommand
{
    private const string ConstDescription = "Observe a file for changes (rather than growth).";

    private readonly InlineDiffBuilder _diffBuilder = new(new Differ());

    private readonly ClearFlag _clearFlag = new();
    private readonly DiffFlag _diffFlag = new();
    private readonly NoAnsiFlag _noAnsiFlag = new();
    private readonly NoFooterFlag _noFooterFlag = new();
    private readonly NoHeaderFlag _noHeaderFlag = new();

    private readonly GrepOption _grepOption = new();
    private readonly HeadOption _headOption = new();
    private readonly IntervalOption _intervalOption = new();
    private readonly TailOption _tailOption = new();

    private readonly FileNameArgument _fileNameArgument = new();

    public DefaultCommand() : base(ConstDescription.WrapLongLines())
    {
        Options.Add(_clearFlag);
        Options.Add(_diffFlag);
        Options.Add(_noFooterFlag);
        Options.Add(_noHeaderFlag);

        Options.Add(_grepOption);
        Options.Add(_headOption);
        Options.Add(_intervalOption);
        Options.Add(_tailOption);

        Arguments.Add(_fileNameArgument);

        SetAction(ExecuteAction);
    }

    public int Execute(string[] args)
    {
        return Parse(args).Invoke();
    }

    private int ExecuteAction(ParseResult parseResult)
    {
        if (parseResult.Errors.Any())
        {
            Console.Error.WriteLine(string.Join(Environment.NewLine, parseResult.Errors.Select(x => x.Message)));
            return ExitCodes.ParseFailure;
        }

        string filePath = parseResult.GetValue(_fileNameArgument)!;
        Regex filterRegex = new Regex(parseResult.GetValue(_grepOption)!);
        int delayMs = parseResult.GetValue(_intervalOption);
        int headLines = parseResult.GetValue(_headOption);
        int tailLines = parseResult.GetValue(_tailOption);
        bool doDiff = parseResult.GetValue(_diffFlag);
        bool doAnsi = !parseResult.GetValue(_noAnsiFlag);
        bool doHeader = !parseResult.GetValue(_noHeaderFlag);
        bool doFooter = !parseResult.GetValue(_noFooterFlag);
        bool doClear = parseResult.GetValue(_clearFlag);

        string previousGreppedContent = string.Empty;

        while (true)
        {
            string fileContent;

            try
            {
                fileContent = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Could not access the file \"{filePath}\". We will try again in {delayMs}ms. Error: {Environment.NewLine}{ex}");
                Thread.Sleep(delayMs);
                continue;
            }

            string greppedOutput = ApplyGrep(fileContent, filterRegex);

            var diffedOutput = doDiff
                ? ApplyDiff(previousGreppedContent, greppedOutput, doAnsi)
                : greppedOutput;

            string headedOutput = string.Join('\n', diffedOutput.Split('\n').Take(headLines));
            string tailedOutput = string.Join('\n', headedOutput.Split('\n').TakeLast(tailLines));

            if (doClear)
            {
                Console.Clear();
            }

            DateTime now = DateTime.Now;
            if (doHeader) Console.WriteLine($"=== START \"{filePath}\" at {now:O} ===");
            Console.WriteLine(tailedOutput);
            if (doFooter) Console.WriteLine($"=== END   \"{filePath}\" at {now:O} ===");

            previousGreppedContent = greppedOutput;

            Thread.Sleep(delayMs);
        }
    }

    private static string ApplyGrep(string source, Regex pattern)
    {
        return source.Split("\n")
            .Where(line => pattern.IsMatch(line))
            .Aggregate(string.Empty, (current, line) => current + line + '\n');
    }

    private string ApplyDiff(string oldContent, string newContent, bool doAnsi)
    {
        StringBuilder sb = new();
        foreach (var line in _diffBuilder.BuildDiffModel(oldContent, newContent).Lines)
        {
            switch (line.Type)
            {
                case ChangeType.Deleted:

                    sb.AppendLine(doAnsi
                        ? $"{Ansi.Red}- {line.Text}{Ansi.Reset}"
                        : $"- {line.Text}");
                    break;
                case ChangeType.Inserted:
                    sb.AppendLine(doAnsi
                        ? $"{Ansi.Green}+ {line.Text}{Ansi.Reset}"
                        : $"+ {line.Text}");
                    break;
                case ChangeType.Modified:
                    sb.AppendLine(doAnsi
                        ? $"{Ansi.Blue}~ {line.Text}{Ansi.Reset}"
                        : $"~ {line.Text}");
                    break;
                case ChangeType.Unchanged:
                case ChangeType.Imaginary:
                default:
                    sb.AppendLine("  " + line.Text);
                    break;
            }
        }

        return sb.ToString();
    }
}
