using System;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation;

// We are disabling NRTs here because otherwise, we have many issues
// with generic parameters that might be null. DTOs are usually deserialized
// so e.g. even string properties that are marked as non-null might actually
// be null.
#nullable disable

public static class Checks
{
    public static Check<T> IsNotNull<T>(this Check<T> check, string message = null)
        where T : class
    {
        if (check.Value is null)
            check.AddIsNotNullError(message);
        return check;
    }

    public static Check<string> IsNotNullOrWhiteSpace(this Check<string> check, string message = null)
    {
        if (check.IsNotNull().HasErrors)
            return check;

        if (check.Value.IsNullOrWhiteSpace())
            check.AddIsNotNullOrWhiteSpaceError(message);

        return check;
    }

    public static Check<T> GreaterThan<T>(this Check<T> check, T other, string message = null)
        where T : IComparable<T>
    {
        other.MustNotBeNullReference();
        
        if (check.Value is null)
            return check;

        if (check.Value.CompareTo(other) <= 0)
            check.AddIsGreaterThanError(other, message);

        return check;
    }

    public static string TrimAndCheckNotWhiteSpace(this Check<string> check, string message = null)
    {
        if (check.Value is null)
        {
            check.AddIsNotNullError(message);
            return null;
        }

        var value = check.Value;
        var span = value.AsSpan().Trim();
        if (span.IsEmpty)
        {
            check.AddIsEmptyOrWhiteSpaceError(message);
            return check.Value;
        }
        
        if (value.Length != span.Length)
        {
            value = span.ToString();
            check = check.WithNewValue(value);
        }

        return check.Value;
    }
}