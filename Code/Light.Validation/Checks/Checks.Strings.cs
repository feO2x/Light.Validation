using System;
using System.Text.RegularExpressions;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation.Checks;

public static partial class Checks
{
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

        if (!regex.IsMatch(check.Value))
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
    /// Thrown when <paramref name="regex"/> or <paramref name="errorMessageFactory"/> are null.
    /// </exception>
    public static Check<string> IsMatching(this Check<string> check,
                                           Regex regex,
                                           Func<Check<string>, Regex, string> errorMessageFactory)
    {
        regex.MustNotBeNull();

        if (!regex.IsMatch(check.Value))
            check.AddError(regex, errorMessageFactory);
        return check;
    }
}