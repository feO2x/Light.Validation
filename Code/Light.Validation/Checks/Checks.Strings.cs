using System;
using System.Text.RegularExpressions;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation.Checks;

public static partial class Checks
{
    /// <summary>
    /// Normalizes the specified string. This is done by performing two steps:
    /// <list type="number">
    /// <item>
    /// Checking for null. If the string is actually null, a check with an
    /// empty string will be returned so that consecutive checks will not throw.
    /// </item>
    /// <item>
    /// Trimming the string. White space at the beginning and end will be removed
    /// and a new string will be allocated if necessary.
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    public static Check<string> Normalize(this Check<string> check)
    {
        if (check.IsValueNull)
            return check.WithNewValue(string.Empty);

        var readOnlySpan = check.Value.AsSpan().Trim();
        if (readOnlySpan.IsEmpty)
            return check.WithNewValue(string.Empty);

        return readOnlySpan.Length == check.Value.Length ? check : check.WithNewValue(readOnlySpan.ToString());
    }
    
    /// <summary>
    /// Checks if the specified string is not null, empty, or contains only white space, or
    /// otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    public static Check<string> IsNotNullOrWhiteSpace(this Check<string> check, string? message = null)
    {
        if (check.Value.IsNullOrWhiteSpace())
            check.AddNotNullOrWhiteSpaceError(message);

        return check;
    }

    /// <summary>
    /// Checks if the specified string is not null, empty, or contains only white space, or
    /// otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<string> IsNotNullOrWhiteSpace(this Check<string> check,
                                                      Func<Check<string>, string> errorMessageFactory)
    {
        if (check.Value.IsNullOrWhiteSpace())
            check.AddError(errorMessageFactory);
        return check;
    }

    /// <summary>
    /// Checks if the specified string matches the specified regular expression, or otherwise
    /// adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="regex">The regular expression that is used to check the string.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="regex" /> is null.</exception>
    public static Check<string> IsMatching(this Check<string> check, Regex regex, string? message = null)
    {
        regex.MustNotBeNull();

        if (!check.IsValueNull && !regex.IsMatch(check.Value))
            check.AddRegexMustMatchError(message);

        return check;
    }

    /// <summary>
    /// Checks if the specified string matches the specified regular expression, or otherwise adds the
    /// error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="regex">The regular expression that is used to check the string.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="regex" /> or <paramref name="errorMessageFactory" /> are null.
    /// </exception>
    public static Check<string> IsMatching(this Check<string> check,
                                           Regex regex,
                                           Func<Check<string>, Regex, string> errorMessageFactory)
    {
        regex.MustNotBeNull();

        if (!check.IsValueNull && !regex.IsMatch(check.Value))
            check.AddError(errorMessageFactory, regex);
        return check;
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
    public static Check<string> IsEmail(this Check<string> check, string? message = null)
    {
        if (!check.Value.IsEmail())
            check.AddEmailError(message);
        return check;
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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory"/> is null.</exception>
    public static Check<string> IsEmail(this Check<string> check, Func<Check<string>, string> errorMessageFactory)
    {
        if (!check.Value.IsEmail())
            check.AddError(errorMessageFactory);
        return check;
    }
}