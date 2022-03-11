using System;
using Light.GuardClauses;

namespace Light.Validation.Tools;

/// <summary>
/// Provides extension methods that add error messages to the validation context.
/// </summary>
public static class Errors
{
    /// <summary>
    /// Adds the "Not Null" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="TCheck">The struct type that implements <see cref="ICheck" />.</typeparam>
    public static void AddNotNullError<TCheck>(this TCheck check, string? message = null)
        where TCheck : struct, ICheck
    {
        message ??= string.Format(check.Context.ErrorTemplates.NotNull, check.Key);
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Equal To" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="T">The type of the value.</typeparam>
    public static void AddEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.EqualTo,
            check.Key,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Not Equal To" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="T">The type of the value.</typeparam>
    public static void AddNotEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.NotEqualTo,
            check.Key,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Not Empty GUID" error message to the context.
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
    /// Adds the "Regex Must Match" error message to the context.
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

    /// <summary>
    /// Adds the "Not Null Or White Space" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static void AddNotNullOrWhiteSpaceError(this Check<string> check, string? message = null)
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.NotNullOrWhiteSpace,
            check.Key
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Greater Than" error message to the context.
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
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Greater Than Or Equal" To error message to the context.
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
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Less Than" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="T">The type of the value.</typeparam>
    public static void AddLessThanError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.LessThan,
            check.Key,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Less Than Or Equal To" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    /// <typeparam name="T">The type of the value.</typeparam>
    public static void AddLessThanOrEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.LessThanOrEqualTo,
            check.Key,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Is In Range" error message to the context.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">The range whose boundaries were violated.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static void AddInRangeError<T>(this Check<T> check, Range<T> range, string? message = null)
        where T : IComparable<T>
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.InRange,
            check.Key,
            check.Context.ErrorTemplates.FormatRange(range)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Is Not In Range" error message to the context.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">The range whose boundaries were violated.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static void AddNotInRangeError<T>(this Check<T> check, Range<T> range, string? message = null)
        where T : IComparable<T>
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.NotInRange,
            check.Key,
            check.Context.ErrorTemplates.FormatRange(range)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Email" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static void AddEmailError(this Check<string> check, string? message = null)
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.Email,
            check.Key
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Longer than" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="length">The comparative length value.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static void AddLongerThanError(this Check<string> check, int length, string? message = null)
    {
        message ??= string.Format(
            check.Context.ErrorTemplates.LongerThan,
            check.Key,
            check.Context.ErrorTemplates.FormatParameter(length)
        );
        check.AddError(message);
    }

    /// <summary>
    /// Adds the "Count" error message to the context. Depending on the <paramref name="count" />,
    /// the <see cref="ErrorTemplates.CountSingular" /> or <see cref="ErrorTemplates.CountMultiple" />
    /// error template is chosen (if message is not null).
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="count">The count the collection was compared against.</param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddCountError<T>(this Check<T> check, int count, string? message = null)
    {
        message ??= count == 1 ?
                        string.Format(check.Context.ErrorTemplates.CountSingular, check.Key) :
                        string.Format(check.Context.ErrorTemplates.CountMultiple, check.Key, count.ToString());
        check.AddError(message);
    }

    /// <summary>
    /// Adds the error message to the context, using the specified error message factory.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked.</param>
    /// <param name="errorMessageFactory">The delegate that receives the check and creates an error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
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