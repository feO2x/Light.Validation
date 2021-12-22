using System;
using Light.GuardClauses;
using Light.GuardClauses.Exceptions;

namespace Light.Validation.Tools;

public static class StringExtensions
{
    public static string NormalizeLastSectionToLowerCamelCase(this string value)
    {
        value.MustNotBeNullOrWhiteSpace();

        var indexOfDot = value.LastIndexOf('.');
        if (indexOfDot == -1)
            return EnsureFirstLetterIsLowerCase(value);

        var startIndex = indexOfDot + 1;
        if (startIndex == value.Length)
            throw new StringException(nameof(value), $"The value \"{value}\" cannot be normalized because it ends with a dot");

        var readOnlySpan = value.AsSpan(startIndex);
        Span<char> span = stackalloc char[readOnlySpan.Length];
        readOnlySpan.CopyTo(span);
        span[0] = char.ToLowerInvariant(span[0]);
        return span.ToString();
    }
    
    public static string EnsureFirstLetterIsLowerCase(this string value)
    {
        value.MustNotBeNullOrWhiteSpace();

        if (char.IsLower(value[0]))
            return value;

        Span<char> span = stackalloc char[value.Length];
        value.AsSpan().CopyTo(span);
        span[0] = char.ToLowerInvariant(value[0]);
        return span.ToString();
    }

    public static string EnsureFirstLetterIsUpperCase(this string value)
    {
        value.MustNotBeNullOrWhiteSpace();

        if (char.IsUpper(value[0]))
            return value;

        Span<char> span = stackalloc char[value.Length];
        value.AsSpan().CopyTo(span);
        span[0] = char.ToUpperInvariant(value[0]);
        return span.ToString();
    }
}