using System;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation.Checks;

// We are disabling NRTs here because otherwise, we have many issues
// with generic parameters that might be null. DTOs are usually deserialized
// by a serializer that does not care about NRTs, so e.g. even string properties
// that are marked as non-null might actually be null.
#nullable disable

/// <summary>
/// Provides extensions for the <see cref="Check{T}"/> structure
/// that allow easy validation of values.
/// </summary>
public static class Checks
{
    /// <summary>
    /// Checks if the specified value is not null, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public static Check<T> IsNotNull<T>(this Check<T> check, string message = null)
        where T : class
    {
        if (check.Value is null)
            check.AddNotNullError(message);
        return check;
    }

    /// <summary>
    /// Checks if the specified value is not null, or otherwise adds the error message that was created
    /// from the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory"/> is null.</exception>
    public static Check<T> IsNotNull<T>(this Check<T> check, Func<Check<T>, string> errorMessageFactory)
        where T : class
    {
        if (check.Value is null)
            check.AddError(errorMessageFactory);
        return check;
    }

    /// <summary>
    /// Checks if the specified nullable value is not null, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public static Check<T?> IsNotNull<T>(this Check<T?> check, string message = null)
        where T : struct
    {
        if (check.Value == null)
            check.AddNotNullError(message);
        return check;
    }
    
    /// <summary>
    /// Checks if the specified value is not null, or otherwise adds the error message that was created
    /// from the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory"/> is null.</exception>
    public static Check<T?> IsNotNull<T>(this Check<T?> check, Func<Check<T?>, string> errorMessageFactory)
        where T : struct
    {
        if (check.Value == null)
            check.AddError(errorMessageFactory);
        return check;
    }

    public static Check<string> IsNotNullOrWhiteSpace(this Check<string> check, string message = null)
    {
        if (check.IsNotNull().HasError)
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
            check.AddNotNullError(message);
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