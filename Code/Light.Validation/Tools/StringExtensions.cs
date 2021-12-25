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
    /// when there are actually characters after the last dot).
    /// In any case, this method will ensure that the first character
    /// is lowercase.
    /// </summary>
    /// <param name="key">The value that should be normalized.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key" /> is null.</exception>
    public static string NormalizeLastSectionToLowerCamelCase(this string key)
    {
        key.MustNotBeNull();

        var readOnlySpan = key.AsSpan().Trim();
        if (readOnlySpan.IsEmpty)
            return string.Empty;

        var indexOfDot = readOnlySpan.GetLastIndexOfDot();
        if (indexOfDot != -1)
        {
            var startIndex = indexOfDot + 1;
            readOnlySpan = readOnlySpan.Slice(startIndex);
        }
        else if (readOnlySpan.Length == key.Length &&
                 readOnlySpan[0].IsStartingCharacterForLowerCamelCase())
        {
            return key;
        }

        Span<char> span = stackalloc char[readOnlySpan.Length];
        readOnlySpan.CopyTo(span);
        span[0] = char.ToLowerInvariant(span[0]);
        return span.ToString();
    }

    private static int GetLastIndexOfDot(this ReadOnlySpan<char> span)
    {
        // We only want to find dots that are not the last
        // character of the input key. That's why we start at
        // span.Length - 2.
        for (var i = span.Length - 2; i >= 0; i--)
        {
            if (span[i] == '.')
                return i;
        }

        return -1;
    }

    private static bool IsStartingCharacterForLowerCamelCase(this char character) =>
        !char.IsLetter(character) || char.IsLower(character);
}