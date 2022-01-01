using System;

namespace Light.Validation.Localization;

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
public record ErrorTemplates
{
    /// <summary>
    /// Gets the default error templates.
    /// </summary>
    public static ErrorTemplates Default { get; } = new ();
    
    /// <summary>
    /// Gets or sets the template for the Not Null error message.
    /// The default value is "{0} must not be null.".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} key</item>
    /// </list>
    /// </summary>
    public string NotNull { get; set; } = "{0} must not be null.";

    /// <summary>
    /// Gets or sets the template for the Greater Than error message.
    /// The default value is "{0} must be greater than {1}.".
    /// This template takes two parameters:
    /// <list type="bullet">
    /// <item>{0} key</item>
    /// <item>{1} comparative value</item>
    /// </list>
    /// </summary>
    public string GreaterThan { get; set; } = "{0} must be greater than {1}.";

    /// <summary>
    /// Gets or sets the template for the Not Empty GUID error message.
    /// The default value is "{0} must not be an empty GUID.".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} key</item>
    /// </list>
    /// </summary>
    public string NotEmptyGuid { get; set; } = "{0} must not be an empty GUID.";

    /// <summary>
    /// Gets or sets the template for the Regex Must Match error message.
    /// The default value is "{0} must match the required pattern.".
    /// This template takes one parameter:
    /// <list type="bullet">
    /// <item>{0} key</item>
    /// </list>
    /// </summary>
    public string RegexMustMatch { get; set; } = "{0} must match the required pattern.";
}