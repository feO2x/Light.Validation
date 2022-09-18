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
    public static Check<T> AddNotNullError<T>(this Check<T> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(check.Context.ErrorTemplates.NotNull, check.DisplayName);
        return check.AddError(message);
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
    public static Check<T> AddEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.EqualTo,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        return check.AddError(message);
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
    public static Check<T> AddNotEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.NotEqualTo,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Not Empty GUID" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<Guid> AddNotEmptyGuidError(this Check<Guid> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(check.Context.ErrorTemplates.NotEmptyGuid, check.DisplayName);
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Regex Must Match" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddRegexMustMatchError(this Check<string> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(check.Context.ErrorTemplates.RegexMustMatch, check.DisplayName);
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Not Null Or White Space" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddNotNullOrWhiteSpaceError(this Check<string> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.NotNullOrWhiteSpace,
            check.DisplayName
        );
        return check.AddError(message);
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
    public static Check<T> AddGreaterThanError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.GreaterThan,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        return check.AddError(message);
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
    public static Check<T> AddGreaterThanOrEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.GreaterThanOrEqualTo,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        return check.AddError(message);
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
    public static Check<T> AddLessThanError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.LessThan,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        return check.AddError(message);
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
    public static Check<T> AddLessThanOrEqualToError<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.LessThanOrEqualTo,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(comparativeValue)
        );
        return check.AddError(message);
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
    public static Check<T> AddInRangeError<T>(this Check<T> check, Range<T> range, string? message = null)
        where T : IComparable<T>
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.InRange,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatRange(range)
        );
        return check.AddError(message);
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
    public static Check<T> AddNotInRangeError<T>(this Check<T> check, Range<T> range, string? message = null)
        where T : IComparable<T>
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.NotInRange,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatRange(range)
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Email" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddEmailError(this Check<string> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.Email,
            check.DisplayName
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Longer Than" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="length">The comparative length value.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddLongerThanError(this Check<string> check, int length, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.LongerThan,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(length)
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Shorter Than" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="length">The comparative length value.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddShorterThanError(this Check<string> check, int length, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.ShorterThan,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatParameter(length)
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Length In Range" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">The range whose boundaries were violated.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddLengthInRangeError(this Check<string> check, Range<int> range, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.LengthInRange,
            check.DisplayName,
            check.Context.ErrorTemplates.FormatRange(range)
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Only Digits" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddOnlyDigitsError(this Check<string> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.OnlyDigits,
            check.DisplayName
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Only Letters And Digits" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<string> AddOnlyLettersAndDigitsError(this Check<string> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.OnlyLettersAndDigits,
            check.DisplayName
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Count" error message to the context. Depending on the <paramref name="count" />,
    /// the <see cref="ErrorTemplates.CountSingular" /> or <see cref="ErrorTemplates.CountMultiple" />
    /// error template is chosen (if message is null).
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="count">The count the collection was compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<T> AddCountError<T>(this Check<T> check, int count, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= count == 1 ?
                        string.Format(check.Context.ErrorTemplates.CountSingular, check.DisplayName) :
                        string.Format(check.Context.ErrorTemplates.CountMultiple, check.DisplayName, count.ToString());
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Minimum Count" error message to the context. Depending on the <paramref name="minimumCount" />,
    /// the <see cref="ErrorTemplates.MinimumCountSingular" /> or <see cref="ErrorTemplates.MinimumCountMultiple" />
    /// error template is chosen (if message is null).
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="minimumCount">The minimum count the collection was compared against.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<T> AddMinimumCountError<T>(this Check<T> check, int minimumCount, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= minimumCount == 1 ?
                        string.Format(check.Context.ErrorTemplates.MinimumCountSingular, check.DisplayName) :
                        string.Format(check.Context.ErrorTemplates.MinimumCountMultiple, check.DisplayName, minimumCount.ToString());
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the "Try Parse to Enum" error message to the context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message (optional). If null is provided, an error message will be
    /// generated from the error templates associated with the context.
    /// </param>
    public static Check<T> AddTryParseToEnumError<T>(this Check<T> check, string? message = null)
    {
        check = check.NormalizeKeyIfNecessary();
        message ??= string.Format(
            check.Context.ErrorTemplates.TryParseToEnum,
            check.DisplayName
        );
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the error message to the context, using the specified error message factory.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked.</param>
    /// <param name="errorMessageFactory">The delegate that receives the check and creates an error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> CreateAndAddError<T>(this Check<T> check, Func<Check<T>, string> errorMessageFactory)
    {
        errorMessageFactory.MustNotBeNull();

        check = check.NormalizeKeyIfNecessary();
        var message = errorMessageFactory(check);
        return check.AddError(message);
    }

    /// <summary>
    /// Adds the error message to the context, using the specified error message factory.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked.</param>
    /// <param name="errorMessageFactory">The delegate that receives the check and creates an error message.</param>
    /// <param name="comparativeValue">A comparative value that was used to validate the value.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <typeparam name="TParameter">The type of the comparative value.</typeparam>
    public static Check<T> CreateAndAddError<T, TParameter>(this Check<T> check,
                                                            Func<Check<T>, TParameter, string> errorMessageFactory,
                                                            TParameter comparativeValue)
    {
        errorMessageFactory.MustNotBeNull();

        check = check.NormalizeKeyIfNecessary();
        var message = errorMessageFactory(check, comparativeValue);
        return check.AddError(message);
    }
}