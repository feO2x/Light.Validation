using System;
using Light.GuardClauses;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.Validation.Tools;

/// <summary>
/// Provides extension methods that add error messages to the validation context.
/// </summary>
public static class Errors
{
    /// <summary>
    /// Adds the Not Null error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="TCheck">The struct type that implements <see cref="ICheck"/>.</typeparam>
    public static void AddNotNullError<TCheck>(this TCheck check, string? message = null)
        where TCheck : struct, ICheck
    {
        message ??= string.Format(check.Context.ErrorTemplates.NotNull, check.Key);
        check.AddError(message);
    }

    /// <summary>
    /// Adds the Not Empty GUID error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static void AddNotEmptyGuidError(this Check<Guid> check, string? message = null)
    {
        message ??= string.Format(check.Context.ErrorTemplates.NotEmptyGuid, check.Key);
        check.AddError(message);
    }

    /// <summary>
    /// Adds the Regex Must Match error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static void AddRegexMustMatchError(this Check<string> check, string? message = null)
    {
        message ??= string.Format(check.Context.ErrorTemplates.RegexMustMatch, check.Key);
        check.AddError(message);
    }

    public static void AddIsNotNullOrWhiteSpaceError(this Check<string> check, string? message = null)
    {
        message ??= $"{check.Key} must not be empty or contain only white space.";
        check.AddError(message);
    }

    /// <summary>
    /// Adds the Greater Than error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="T">The type of the value.</typeparam>
    public static void AddGreaterThanError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.GreaterThan,
            check.Key,
            Formatter.Format(comparativeValue)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the Greater Than Or Equal To error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="T">The type of the value.</typeparam>
    public static void AddGreaterThanOrEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.GreaterThanOrEqualTo,
            check.Key,
            Formatter.Format(comparativeValue)
        );
        check.AddError(message);
    }

    public static void AddIsEmptyOrWhiteSpaceError(this Check<string> check, string? message = null)
    {
        message ??= $"{check.Key} must not be empty or contain only white space";
        check.AddError(message);
    }

    /// <summary>
    /// Adds the error message to the context, using the specified error message factory.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked.</param>
    /// <param name="errorMessageFactory">The delegate that receives the check and creates an error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory"/> is null.</exception>
    public static void AddError<T>(this Check<T> check, Func<Check<T>, string> errorMessageFactory)
    {
        errorMessageFactory.MustNotBeNull();

        var message = errorMessageFactory(check);
        check.AddError(message);
    }

    /// <summary>
    /// Adds the error message to the context, using the specified error message factory.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked.</param>
    /// <param name="errorMessageFactory">The delegate that receives the check and creates an error message.</param>
    /// <param name="comparativeValue">A comparative value that was used to validate the value.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <typeparam name="TParameter">The type of the comparative value.</typeparam>
    public static void AddError<T, TParameter>(this Check<T> check,
                                               Func<Check<T>, TParameter, string> errorMessageFactory,
                                               TParameter comparativeValue)
    {
        errorMessageFactory.MustNotBeNull();

        var message = errorMessageFactory(check, comparativeValue);
        check.AddError(message);
    }
}