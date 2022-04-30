using System;
using Light.GuardClauses;

namespace Light.Validation.Tools;

/// <summary>
/// Provides extension methods for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Checks if the key contains dots. If yes, the substring
    /// after the last dot will be taken instead of the whole string (only
    /// when there are actually characters after the last dot). The key is
    /// trimmed to remove leading and trailing white space.
    /// </summary>
    /// <param name="key">The value that should be normalized.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key" /> is null.</exception>
    public static string GetSectionAfterLastDot(this string key)
    {
        key.MustNotBeNull();

        var readOnlySpan = key.AsSpan().Trim();
        if (readOnlySpan.IsEmpty)
            return string.Empty;

        var indexOfDot = readOnlySpan.GetLastIndexOfDot();
        if (indexOfDot == -1)
            return readOnlySpan.Length == key.Length ? key : readOnlySpan.ToString();

        return readOnlySpan.Slice(indexOfDot + 1).ToString();
    }

    private static int GetLastIndexOfDot(this ReadOnlySpan<char> span)
    {
        // We only want to find dots that are not the last
        // character of the input key.
        var index = span.LastIndexOf('.');
        return index == -1 || index == span.Length - 1 ? -1 : index;
    }

    /// <summary>
    /// Checks if the specified value is an email address. This is done by checking if the string
    /// contains only one "@" sign which must be somewhere in between (not at the beginning and
    /// not at the end). This is the default way to check an email address in ASP.NET Core, you will
    /// usually have to send a mail to the address to verify if it exists or not. No complex regular
    /// expression is used in this check.
    /// </summary>
    /// <param name="value">The string to be checked.</param>
    public static bool IsEmail(this string? value)
    {
        if (value is null)
            return false;

        if (value.Length < 3)
            return false;

        var indexOfAtSign = value.IndexOf('@');
        if (indexOfAtSign is -1 or 0 || indexOfAtSign == value.Length - 1)
            return false;

        var lastIndexOfAtSign = value.LastIndexOf('@');
        return indexOfAtSign == lastIndexOfAtSign;
    }

    /// <summary>
    /// Normalizes a string value. This is done by performing two steps:
    /// <list type="number">
    /// <item>
    /// Checking for null: in this case, an empty string will be returned.
    /// </item>
    /// <item>
    /// Trimming the string: white space at the beginning and end will be removed
    /// and a new string will be allocated if necessary.
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="value">The value to be normalized.</param>
    public static string NormalizeString(this string? value)
    {
        if (value is null)
            return string.Empty;

        var trimmedSpan = value.AsSpan().Trim();
        if (trimmedSpan.IsEmpty)
            return string.Empty;

        return trimmedSpan.Length == value.Length ?
                   value :
                   trimmedSpan.ToString();
    }
}