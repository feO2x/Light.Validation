﻿using System;
using System.Globalization;

namespace Light.Validation.Tools;

/// <summary>
/// <para>
/// Provides the format strings that are used when creating error messages.
/// </para>
/// <para>
/// You can instantiate this class and provide values for the different
/// properties to enable localization for error messages. We recommend
/// to provide this instance as a singleton. You can inject it in the
/// constructor of your validation context.
/// </para>
/// </summary>
public class ErrorTemplates : ExtensibleObject
{
    /// <summary>
    /// Initializes a new instance of <see cref="ErrorTemplates" />.
    /// </summary>
    /// <param name="other">Another extensible object whose attached objects will be shallow-copied to this instance.</param>
    public ErrorTemplates(ExtensibleObject? other = null)
        : base(other) { }

    /// <summary>
    /// Gets the default error templates.
    /// </summary>
    public static ErrorTemplates Default { get; } = new ();

    /// <summary>
    /// Gets the culture info that is used to format parameters for string.Format.
    /// The default value is the invariant culture.
    /// </summary>
    public CultureInfo CultureInfo { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    /// Gets the template for the "Not Null" error message.
    /// The default value is "{0} must not be null".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string NotNull { get; set; } = "{0} must not be null";

    /// <summary>
    /// Gets the template for the "Equal To" error message.
    /// The default value is "{0} must be {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} comparative value</item>
    /// </list>
    /// </summary>
    public string EqualTo { get; set; } = "{0} must be {1}";

    /// <summary>
    /// Gets the template for the "Not Equal To" error message.
    /// The default value is "{0} must not be {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} comparative value</item>
    /// </list>
    /// </summary>
    public string NotEqualTo { get; set; } = "{0} must not be {1}";

    /// <summary>
    /// Gets the template for the "Greater Than" error message.
    /// The default value is "{0} must be greater than {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} comparative value</item>
    /// </list>
    /// </summary>
    public string GreaterThan { get; set; } = "{0} must be greater than {1}";

    /// <summary>
    /// Gets the template for the "Greater Than Or Equal To" error message.
    /// The default value is "{0} must be greater than or equal to {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} comparative value</item>
    /// </list>
    /// </summary>
    public string GreaterThanOrEqualTo { get; set; } = "{0} must be greater than or equal to {1}";

    /// <summary>
    /// Gets the template for the "Less Than" error message.
    /// The default value is "{0} must be less than {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} comparative value</item>
    /// </list>
    /// </summary>
    public string LessThan { get; set; } = "{0} must be less than {1}";

    /// <summary>
    /// Gets the template for the "Less Than Or Equal To" error message.
    /// The default value is "{0} must be less than or equal to {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} comparative value</item>
    /// </list>
    /// </summary>
    public string LessThanOrEqualTo { get; set; } = "{0} must be less than or equal to {1}";

    /// <summary>
    /// Gets the template for the "Is In Range" error message.
    /// The default value is "{0} must be in range from {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} the Range&lt;T&gt; instance where, by default, CreateRangeDescriptionText is called upon. You can override FormatRange to change this behavior.</item>
    /// </list>
    /// </summary>
    public string InRange { get; set; } = "{0} must be in range from {1}";

    /// <summary>
    /// Gets the template for the "Is In Range" error message.
    /// The default value is "{0} must not be in range from {1}".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} the Range&lt;T&gt; instance where, by default, CreateRangeDescriptionText is called upon. You can override FormatRange to change this behavior.</item>
    /// </list>
    /// </summary>
    public string NotInRange { get; set; } = "{0} must not be in range from {1}";

    /// <summary>
    /// Gets the template for the "Not Empty GUID" error message.
    /// The default value is "{0} must not be an empty GUID".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string NotEmptyGuid { get; set; } = "{0} must not be an empty GUID";

    /// <summary>
    /// Gets the template for the "Not Null Or White Space" error message.
    /// The default value is "{0} must not be empty".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string NotNullOrWhiteSpace { get; set; } = "{0} must not be empty";

    /// <summary>
    /// Gets the template for the "Regex Must Match" error message.
    /// The default value is "{0} must match the required pattern".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string RegexMustMatch { get; set; } = "{0} must match the required pattern";

    /// <summary>
    /// Gets the template for the "Email" error message.
    /// The default value is "{0} must be an email address".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string Email { get; set; } = "{0} must be an email address";

    /// <summary>
    /// Gets the template for the "Longer Than" error message.
    /// The default value is "{0} must be longer than {1} characters".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} the comparative length value</item>
    /// </list>
    /// </summary>
    public string LongerThan { get; set; } = "{0} must be longer than {1} characters";

    /// <summary>
    /// Gets the template for the "Shorter Than" error message.
    /// The default value is "{0} must be shorter than {1} characters".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} the comparative length value</item>
    /// </list>
    /// </summary>
    public string ShorterThan { get; set; } = "{0} must be shorter than {1} characters";

    /// <summary>
    /// Gets the template for the "Length In Range" error message.
    /// The default value is "{0} must have {1} characters".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} the Range&lt;T&gt; instance where, by default, CreateRangeDescriptionText is called upon. You can override FormatRange to change this behavior.</item>
    /// </list>
    /// </summary>
    public string LengthInRange { get; set; } = "{0} must have {1} characters";

    /// <summary>
    /// Gets the template for the "Only Digits" error message.
    /// The default value is "{0} must contain only digits".
    /// The template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string OnlyDigits { get; set; } = "{0} must contain only digits";

    /// <summary>
    /// Gets the template for the "Only Letters and Digits" error message.
    /// The default value is "{0} must contain only digits".
    /// The template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string OnlyLettersAndDigits { get; set; } = "{0} must contain only letters and digits";

    /// <summary>
    /// Gets the template for the "Count Singular" error message.
    /// The default value is "{0} must have 1 item".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string CountSingular { get; set; } = "{0} must have 1 item";

    /// <summary>
    /// Gets the template for the "Count Multiple" error message.
    /// The default value is "{0} must have {1} items".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// <item>{1} count</item>
    /// </list>
    /// </summary>
    public string CountMultiple { get; set; } = "{0} must have {1} items";

    /// <summary>
    /// Gets the template for the "Try Parse To Enum" error message.
    /// The default value is "{0} is not one of the allowed values".
    /// The template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} display name</item>
    /// </list>
    /// </summary>
    public string TryParseToEnum { get; set; } = "{0} must be one of the allowed values";

    /// <summary>
    /// Formats the specified parameter, potentially using the culture info attached to this error templates instance.
    /// </summary>
    public virtual string FormatParameter<T>(T value) => Formatter.Format(value, CultureInfo);

    /// <summary>
    /// Formats the specified range. By default, CreateRangeDescriptionText is called on the instance.
    /// </summary>
    public virtual string FormatRange<T>(Range<T> range)
        where T : IComparable<T> => range.CreateRangeDescriptionText();
}