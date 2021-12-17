using System;
using Light.GuardClauses;

namespace Light.Validation;

public static class StringExtensions
{
    public static string EnsureFirstLetterIsLowerCase(string value)
    {
        value.MustNotBeNullOrWhiteSpace();

        if (char.IsLower(value[0]))
            return value;

        Span<char> span = stackalloc char[value.Length];
        value.AsSpan().CopyTo(span);
        span[0] = char.ToLowerInvariant(value[0]);
        return span.ToString();
    }

    public static string EnsureFirstLetterIsUpperCase(string value)
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