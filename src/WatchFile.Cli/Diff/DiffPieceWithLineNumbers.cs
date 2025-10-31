using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffPlex.DiffBuilder.Model;

namespace Dev.JoshBrunton.WatchFile.Cli.Diff;

internal class DiffPieceWithLineNumbers : DiffPiece
{
    public int OldLineNumber { get; init; }
    public int NewLineNumber { get; init; }

    private DiffPieceWithLineNumbers(DiffPiece source, int oldLineNumber, int newLineNumber)
    {
        Type = source.Type;
        Position = source.Position;
        SubPieces = source.SubPieces;
        Text = source.Text;
        OldLineNumber = oldLineNumber;
        NewLineNumber = newLineNumber;
    }

    public static IEnumerable<DiffPieceWithLineNumbers> ParseList(IEnumerable<DiffPiece> source)
    {
        int oldLineNum = 1;
        int newLineNum = 1;

        foreach (var line in source)
        {
            yield return new DiffPieceWithLineNumbers(line, oldLineNum, newLineNum);

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
