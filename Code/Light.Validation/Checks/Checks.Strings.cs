using System;
using System.Text.RegularExpressions;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation.Checks;

public static partial class Checks
{
    /// <summary>
    /// <para>
    /// Normalizes the specified string. This is done by performing two steps:
    /// <list type="number">
    /// <item>
    /// Checking for null: If the string is actually null, a check with an
    /// empty string will be returned so that consecutive checks will not throw.
    /// </item>
    /// <item>
    /// Trimming the string: white space at the beginning and end will be removed
    /// and a new string will be allocated if necessary.
    /// </item>
    /// </list>
    /// If the check is already short-circuited, nothing will be done.
    /// </para>
    /// <para>
    /// By default, you do not need to call this extension method. String values are automatically
    /// normalized when calling <c>context.Check(dto.SomeString)</c> unless you set
    /// <see cref="ValidationContextOptions.IsNormalizingStringValues" /> to false.
    /// </para>
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    public static Check<string> Normalize(this Check<string> check) =>
        check.IsShortCircuited ? check : check.WithNewValue(check.Value.NormalizeString());

    /// <summary>
    /// Checks if the specified string is not null, empty, or contains only white space, or
    /// otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<string> IsNotNullOrWhiteSpace(this Check<string> check,
                                                      string? message = null,
                                                      bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.Value.IsNullOrWhiteSpace())
            return check;

        check.AddNotNullOrWhiteSpaceError(message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string is not null, empty, or contains only white space, or
    /// otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> IsNotNullOrWhiteSpace(this Check<string> check,
                                                      Func<Check<string>, string> errorMessageFactory,
                                                      bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.Value.IsNullOrWhiteSpace())
            return check;

        check.CreateAndAddError(errorMessageFactory);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// <para>
    /// Checks if the specified string matches the specified regular expression, or otherwise
    /// adds an error message to the validation context.
    /// </para>
    /// <para>
    /// This method calls <see cref="Regex.IsMatch(string)" /> only when the provided string value
    /// is not null (to avoid null-reference-exceptions). If it is null, the error message
    /// will be added to the validation context. You can prevent this behavior by short-circuiting the check
    /// instance passed to this method beforehand.
    /// </para>
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="regex">The regular expression that is used to check the string.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="regex" /> is null.</exception>
    public static Check<string> IsMatching(this Check<string> check,
                                           Regex regex,
                                           string? message = null,
                                           bool shortCircuitOnError = false)
    {
        regex.MustNotBeNull();

        if (check.IsShortCircuited || !check.IsValueNull && regex.IsMatch(check.Value))
            return check;

        check.AddRegexMustMatchError(message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// <para>
    /// Checks if the specified string matches the specified regular expression, or otherwise adds the
    /// error message that was created by the specified factory to the validation context.
    /// </para>
    /// <para>
    /// This method calls <see cref="Regex.IsMatch(string)" /> only when the provided string value
    /// is not null (to avoid null-reference-exceptions). If it is null, the error message
    /// will be added to the validation context. You can prevent this behavior by short-circuiting the check
    /// instance passed to this method beforehand.
    /// </para>
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="regex">The regular expression that is used to check the string.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="regex" /> or <paramref name="errorMessageFactory" /> are null.
    /// </exception>
    public static Check<string> IsMatching(this Check<string> check,
                                           Regex regex,
                                           Func<Check<string>, Regex, string> errorMessageFactory,
                                           bool shortCircuitOnError = false)
    {
        regex.MustNotBeNull();

        if (check.IsShortCircuited || !check.IsValueNull && regex.IsMatch(check.Value))
            return check;

        check.CreateAndAddError(errorMessageFactory, regex);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string looks like an email address, or otherwise
    /// adds an error message to the validation context. This is done by checking if the string
    /// contains only one "@" sign which must be somewhere in between (not at the beginning and
    /// not at the end). This is the default way to check an email address in ASP.NET Core, you will
    /// usually have to send a mail to the address to verify if it exists or not. No complex regular
    /// expression is used in this check.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<string> IsEmail(this Check<string> check,
                                        string? message = null,
                                        bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || check.Value.IsEmail())
            return check;

        check.AddEmailError(message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string looks like an email address, or otherwise adds the
    /// error message that was created by the specified factory to the validation context.
    /// This is done by checking if the string contains only one "@" sign which must be somewhere
    /// in between (not at the beginning and not at the end).
    /// This is the default way to check an email address in ASP.NET Core, you will
    /// usually have to send a mail to the address to verify if it exists or not. No complex regular
    /// expression is used in this check.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> IsEmail(this Check<string> check,
                                        Func<Check<string>, string> errorMessageFactory,
                                        bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || check.Value.IsEmail())
            return check;

        check.CreateAndAddError(errorMessageFactory);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string is longer than the given length, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="length">The comparative length value.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<string> IsLongerThan(this Check<string> check,
                                             int length,
                                             string? message = null,
                                             bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.Length > length)
            return check;

        check.AddLongerThanError(length, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string is longer than the given length, or
    /// otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="length">The comparative length value.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> IsLongerThan(this Check<string> check,
                                             int length,
                                             Func<Check<string>, int, string> errorMessageFactory,
                                             bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.Length > length)
            return check;

        check.CreateAndAddError(errorMessageFactory, length);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string is shorter than the given length, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="length">The comparative length value.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<string> IsShorterThan(this Check<string> check,
                                              int length,
                                              string? message = null,
                                              bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.Length < length)
            return check;

        check.AddShorterThanError(length, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string is shorter than the given length, or
    /// otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="length">The comparative length value.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> IsShorterThan(this Check<string> check,
                                              int length,
                                              Func<Check<string>, int, string> errorMessageFactory,
                                              bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.Length < length)
            return check;

        check.CreateAndAddError(errorMessageFactory, length);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string's length satisfies the limits of the given range,
    /// or otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">
    /// The value describing the allowed lower and upper boundaries.
    /// Use the static <see cref="Range" /> class to fluently create a <see cref="Range{T}" /> instance.
    /// </param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<string> HasLengthIn(this Check<string> check,
                                            Range<int> range,
                                            string? message = null,
                                            bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.IsValueNull && range.IsValueWithinRange(check.Value.Length))
            return check;

        check.AddLengthInRangeError(range, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string's length satisfies the limits of the given range,
    /// or otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">
    /// The value describing the allowed lower and upper boundaries.
    /// Use the static <see cref="Range" /> class to fluently create a <see cref="Range{T}" /> instance.
    /// </param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> HasLengthIn(this Check<string> check,
                                            Range<int> range,
                                            Func<Check<string>, Range<int>, string> errorMessageFactory,
                                            bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited || !check.IsValueNull && range.IsValueWithinRange(check.Value.Length))
            return check;

        check.CreateAndAddError(errorMessageFactory, range);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string contains only digits,
    /// or otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<string> ContainsOnlyDigits(this Check<string> check,
                                                   string? message = null,
                                                   bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited)
            return check;

        if (!check.IsValueNull && check.Value.Length > 0)
        {
            for (var i = 0; i < check.Value.Length; i++)
            {
                if (!check.Value[i].IsDigit())
                    goto StringInvalid;
            }

            return check;
        }

        StringInvalid:
        check.AddOnlyDigitsError(message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string contains only digits,
    /// or otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> ContainsOnlyDigits(this Check<string> check,
                                                   Func<Check<string>, string> errorMessageFactory,
                                                   bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited)
            return check;

        if (!check.IsValueNull && check.Value.Length > 0)
        {
            for (var i = 0; i < check.Value.Length; i++)
            {
                if (!check.Value[i].IsDigit())
                    goto StringInvalid;
            }

            return check;
        }

        StringInvalid:
        check.CreateAndAddError(errorMessageFactory);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string contains only letters and digits,
    /// or otherwise adds an error message to the validation context.
    /// This method uses the <see cref="char.IsLetterOrDigit(char)" /> method internally.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<string> ContainsOnlyLettersAndDigits(this Check<string> check,
                                                             string? message = null,
                                                             bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited)
            return check;

        if (!check.IsValueNull && check.Value.Length > 0)
        {
            for (var i = 0; i < check.Value.Length; i++)
            {
                if (!check.Value[i].IsLetterOrDigit())
                    goto StringInvalid;
            }

            return check;
        }

        StringInvalid:
        check.AddOnlyLettersAndDigitsError(message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified string contains only letters and digits,
    /// or otherwise adds an error message to the validation context.
    /// This method uses the <see cref="char.IsLetterOrDigit(char)" /> method internally.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> ContainsOnlyLettersAndDigits(this Check<string> check,
                                                             Func<Check<string>, string> errorMessageFactory,
                                                             bool shortCircuitOnError = false)
    {
        if (check.IsShortCircuited)
            return check;

        if (!check.IsValueNull && check.Value.Length > 0)
        {
            for (var i = 0; i < check.Value.Length; i++)
            {
                if (!check.Value[i].IsLetterOrDigit())
                    goto StringInvalid;
            }

            return check;
        }

        StringInvalid:
        check.CreateAndAddError(errorMessageFactory);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }
}