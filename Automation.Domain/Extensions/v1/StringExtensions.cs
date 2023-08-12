using System.Text.RegularExpressions;

namespace Automation.Domain.Extensions;

public static partial class StringExtensions
{
    public static string? RemoveLetters(this string input)
    {
        var regex = RegexOnlyNumbers();
        var result = regex.Replace(input, "");
        
        return result;
    }

    [GeneratedRegex("[^0-9]", RegexOptions.Compiled)]
    private static partial Regex RegexOnlyNumbers();
}
