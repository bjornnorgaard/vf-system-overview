using System.Text.RegularExpressions;

namespace Overview;

internal static class StringExtensions
{
    internal static string PruneSpecialCharacters(this string value)
    {
        return Regex.Replace(value, "[^a-zA-Z0-9]", "");
    }
}