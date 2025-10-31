using Dev.JoshBrunton.WatchFile.Cli.Arguments.RootCommands.DefaultCommand;
using Dev.JoshBrunton.WatchFile.Cli.Consts;
using Dev.JoshBrunton.WatchFile.Cli.Flags.RootCommands.DefaultCommand;
using Dev.JoshBrunton.WatchFile.Cli.Options.RootCommands.DefaultCommand;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System.CommandLine;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Dev.JoshBrunton.WatchFile.Cli.Extensions.DiffPlex.Builder.Model;
using Dev.JoshBrunton.WatchFile.Cli.Response;

namespace Dev.JoshBrunton.WatchFile.Cli.Commands.RootCommands;

internal class DefaultCommand : RootCommand
{
    private const string ConstDescription = """
                                            Observe a file for changes (rather than growth).

                                            This command, and all of its subcommands, support the auto-response feature. The contents of the file ~/.config/watch-file/watch-file.rsp (or ~/.config/watch-file/watch-file.<subcommand>.rsp for subcommands) will automatically be appended to the end of any command invoked.
                                            """;

    private readonly AutoRspHelper<DefaultCommand> _autoRspHelper;
    private readonly InlineDiffBuilder _diffBuilder = new(new Differ());

    private readonly ClearFlag _clearFlag = new();
    private readonly DiffFlag _diffFlag = new();
    private readonly NoAnsiFlag _noAnsiFlag = new();
    private readonly NoFooterFlag _noFooterFlag = new();
    private readonly NoHeaderFlag _noHeaderFlag = new();
    private readonly WatchFlag _watchFlag = new();

    private readonly GrepOption _grepOption = new();
    private readonly HeadOption _headOption = new();
    private readonly IntervalOption _intervalOption = new();
    private readonly TailOption _tailOption = new();

    private readonly FileNameArgument _fileNameArgument = new();

    public DefaultCommand() : base(ConstDescription)
    {
        _autoRspHelper = new AutoRspHelper<DefaultCommand>(this);

        Options.Add(_clearFlag);
        Options.Add(_diffFlag);
        Options.Add(_noFooterFlag);
        Options.Add(_noHeaderFlag);
        Options.Add(_watchFlag);

        Options.Add(_grepOption);
        Options.Add(_headOption);
        Options.Add(_intervalOption);
        Options.Add(_tailOption);

        Arguments.Add(_fileNameArgument);

        SetAction(ExecuteAction);
    }

    public int Execute(string[] args)
    {
        _autoRspHelper.MutateArgs(ref args);
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
        bool doWatch = parseResult.GetValue(_watchFlag);

        string? fileDirectory = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (fileDirectory is null)
        {
            Console.Error.WriteLine($"Couldn't determine the directory name from \"{filePath}\".");
            return ExitCodes.FailedToGetDirectory;
        }
        using FileSystemWatcher fsw = new(fileDirectory);
        fsw.Filter = Path.GetFileName(filePath);
        fsw.NotifyFilter = NotifyFilters.LastWrite;

        List<string> previousGreppedContent = [];
        byte[] previousMd5Sum = [];

        while (true)
        {
            DateTime iterationStartDateTime = DateTime.Now;
            string[] fileContent;

            try
            {
                fileContent = File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Could not access the file \"{filePath}\".");
                Console.Error.WriteLine(doWatch 
                    ? "We will try again next time the filesystem reports a change." 
                    : $"We will try again in {delayMs}ms.");
                Console.Error.WriteLine($"Error: {Environment.NewLine}{ex}");
                Thread.Sleep(delayMs);
                continue;
            }

            byte[] md5Sum = MD5.HashData(fileContent.SelectMany(x => x.Select(y => (byte)y)).ToArray());
            if (md5Sum.SequenceEqual(previousMd5Sum))
            {
                continue;
            }

            List<string> greppedOutput = ApplyGrep(fileContent, filterRegex).ToList();

            var diffedOutput = doDiff
                ? ApplyDiff(previousGreppedContent, greppedOutput, doAnsi)
                : greppedOutput;

            IEnumerable<string> headedOutput = diffedOutput.Take(headLines);
            IEnumerable<string> tailedOutput = headedOutput.TakeLast(tailLines);

            if (doClear)
            {
                Console.Clear();
            }

            DateTime now = DateTime.Now;
            var errorMargin = (DateTime.Now - iterationStartDateTime).TotalMilliseconds + (doWatch ? 0 : delayMs);
            if (doHeader) Console.WriteLine($"=== START \"{filePath}\" at {now:O} +-{errorMargin}ms ===");
            foreach (string line in tailedOutput)
            {
                Console.WriteLine(line);
            }
            if (doFooter) Console.WriteLine($"=== END   \"{filePath}\" at {now:O} +-{errorMargin}ms ===");

            previousGreppedContent = greppedOutput;
            previousMd5Sum = md5Sum;

            if (doWatch)
            {
                fsw.WaitForChanged(WatcherChangeTypes.Changed);
            }
            else
            {
                Thread.Sleep(delayMs);
            }
        }
    }

    private static IEnumerable<string> ApplyGrep(string[] source, Regex pattern)
    {
        return source.Where(line => pattern.IsMatch(line));
    }

    private IEnumerable<string> ApplyDiff(IEnumerable<string> oldContent, IEnumerable<string> newContent, bool doAnsi)
    {
        IEnumerable<DiffPiece> lines = _diffBuilder.BuildDiffModel(
            string.Join(Environment.NewLine, oldContent),
            string.Join(Environment.NewLine, newContent)).Lines;
        if (doAnsi)
        {
            lines = lines.Select(x => x.WithAnsi());
        }
        lines = lines.WithLineNumbers();

        return lines.Select(x => x.Text);
    }
}
