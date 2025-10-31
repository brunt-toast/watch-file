using Dev.JoshBrunton.WatchFile.Cli.Consts;
using DiffPlex.DiffBuilder.Model;

namespace Dev.JoshBrunton.WatchFile.Cli.Extensions.DiffPlex.Builder.Model;

internal static class DiffPieceExtensions
{
    public static DiffPiece WithAnsi(this DiffPiece source)
    {
        source.Text = source.Type switch
        {
            ChangeType.Deleted => $"{Ansi.Red}- {source.Text}{Ansi.Reset}",
            ChangeType.Inserted => $"{Ansi.Green}+ {source.Text}{Ansi.Reset}",
            ChangeType.Modified => $"{Ansi.Blue}~ {source.Text}{Ansi.Reset}",
            _ => source.Text
        };

        return source;
    }

    public static IEnumerable<DiffPiece> WithLineNumbers(this IEnumerable<DiffPiece> source)
    {
        int oldLineNum = 1;
        int newLineNum = 1;

        var sourceList = source.ToList();
        int longestNumberRepr = sourceList.Count.ToString().Length;

        foreach (var line in sourceList)
        {
            line.Text =
                $"{oldLineNum.ToString().PadLeft(longestNumberRepr)} {newLineNum.ToString().PadLeft(longestNumberRepr)} {line.Text}";

            yield return line;

            if (line.Type != ChangeType.Inserted)
            {
                oldLineNum++;
            }
            if (line.Type != ChangeType.Deleted)
            {
                newLineNum++;
            }
        }
    }
}
