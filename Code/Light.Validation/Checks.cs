using System;
using Light.GuardClauses;

namespace Light.Validation;

#nullable disable

public static class Checks
{
    public static Check<T> IsNotNull<T>(this Check<T> check)
        where T : class
    {
        if (check.Value is null)
            check.AddError($"{check.Key} must not be null.");
        return check!;
    }

    public static Check<string> IsNotNullOrWhiteSpace(this Check<string> check)
    {
        if (check.IsNotNull().HasErrors)
            return check;

        if (check.Value.IsNullOrWhiteSpace())
            check.AddError($"{check.Key} must not be empty or contain only white space.");

        return check;
    }

    public static Check<T> GreaterThan<T>(this Check<T> check, T other)
        where T : IComparable<T>
    {
        other.MustNotBeNullReference();
        
        if (check.Value is null)
            return check;

        if (check.Value.CompareTo(other) <= 0)
            check.AddError($"{check.Key} must be greater than {other.ToString()}.");

        return check;
    }

    public static string TrimAndCheckNotWhiteSpace(this Check<string> check)
    {
        if (check.IsNotNull().HasErrors)
            return check.Value;

        var value = check.Value;
        var span = value.AsSpan().Trim();
        if (span.IsEmpty)
        {
            check.AddError($"{check.Key} must not be empty or contain only white space");
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