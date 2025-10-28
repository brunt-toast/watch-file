using System.Text;

namespace Dev.JoshBrunton.WatchFile.Cli.Extensions.System;

internal static class StringExtensions
{
    public static string WrapLongLines(this string source, int maxLineLength = 80)
    {
        StringBuilder ret = new();

        int ptr = 0;
        foreach (string word in source.Split(' '))
        {
            if (ptr > maxLineLength)
            {
                ret.Append(Environment.NewLine);
                ptr = 0;
            }

            if (word.Contains('\n'))
            {
                ptr = 0;
            }

            ret.Append(word);
            ret.Append(' ');

            ptr += word.Length;
        }

        return ret.ToString();
    }
}
