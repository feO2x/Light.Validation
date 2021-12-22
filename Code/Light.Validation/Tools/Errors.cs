using System;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.Validation.Tools;

public static class Errors
{
    public static void AddIsNotNullError<T>(this Check<T> check, string? message = null)
        where T : class
    {
        message ??= $"{check.Key} must not be null";
        check.AddError(message);
    }

    public static void AddIsNotNullOrWhiteSpaceError(this Check<string> check, string? message = null)
    {
        message ??= $"{check.Key} must not be empty or contain only white space.";
        check.AddError(message);
    }

    public static void AddIsGreaterThanError<T>(this Check<T> check, T other, string? message = null)
        where T : IComparable<T>
    {
        message ??= $"{check.Key} must be greater than {other.ToStringRepresentation()}.";
        check.AddError(message);
    }

    public static void AddIsEmptyOrWhiteSpaceError(this Check<string> check, string? message = null)
    {
        message ??= $"{check.Key} must not be empty or contain only white space";
        check.AddError(message);
    }
}