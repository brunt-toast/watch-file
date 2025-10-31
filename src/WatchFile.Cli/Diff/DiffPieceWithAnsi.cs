using Dev.JoshBrunton.WatchFile.Cli.Consts;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.JoshBrunton.WatchFile.Cli.Diff;

internal class DiffPieceWithAnsi : DiffPiece
{
    public DiffPieceWithAnsi(DiffPiece source)
    {
        Type = source.Type;
        Position = source.Position;
        SubPieces = source.SubPieces;
        Text = Type switch
        {
            ChangeType.Deleted => $"{Ansi.Red}- {source.Text}{Ansi.Reset}",
            ChangeType.Inserted => $"{Ansi.Green}+ {source.Text}{Ansi.Reset}",
            ChangeType.Modified => $"{Ansi.Blue}~ {source.Text}{Ansi.Reset}", 
            _ => source.Text
        };
    }
}
