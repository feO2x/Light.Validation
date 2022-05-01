using System;
using System.Collections.Generic;
using Light.Validation.Tools;

namespace Light.Validation;

/// <summary>
/// Represents options for the <see cref="ValidationContext" /> class.
/// </summary>
public class ValidationContextOptions : ExtensibleObject
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValidationContextOptions" />.
    /// </summary>
    /// <param name="other">Another extensible object whose attached objects will be shallow-copied to this instance.</param>
    public ValidationContextOptions(ExtensibleObject? other = null)
        : base(other) { }

    /// <summary>
    /// Gets the default validation context options.
    /// </summary>
    public static ValidationContextOptions Default { get; } = new ();

    /// <summary>
    /// The value indicating whether keys are normalized when error messages are created.
    /// The default value is true.
    /// </summary>
    public bool IsNormalizingKeys { get; set; } = true;

    /// <summary>
    /// Gets or sets the delegate that is used to normalize keys.
    /// The default value is null. If no delegate is set,
    /// the default normalization function will be used which is
    /// <see cref="StringExtensions.GetSectionAfterLastDot" />.
    /// </summary>
    public Func<string, string>? NormalizeKey { get; set; }

    /// <summary>
    /// Gets or sets the comparer that is used to compare keys in
    /// the errors dictionary. The default value is null, thus the
    /// EqualityComparer&lt;string&gt;.Default is used by the internal
    /// errors dictionary, comparing strings with ordinal options.
    /// </summary>
    public IEqualityComparer<string>? KeyComparer { get; set; }

    /// <summary>
    /// Gets or sets the value indicating how multiple errors per key are
    /// handled. The default value is "AppendWithNewLine".
    /// </summary>
    public MultipleErrorsPerKeyBehavior MultipleErrorsPerKeyBehavior { get; set; } =
        MultipleErrorsPerKeyBehavior.AppendWithNewLine;

    /// <summary>
    /// Gets the string that is used as new line characters when combining error messages
    /// with the "AppendWithNewLine" behavior. The default value is "\n".
    /// </summary>
    public string NewLine { get; set; } = "\n";

    /// <summary>
    /// Gets or sets the value indicating whether string values will be normalized
    /// when <see cref="ValidationContext.Check{T}" /> is called. The default value is true.
    /// </summary>
    public bool IsNormalizingStringValues { get; set; } = true;

    /// <summary>
    /// Gets or sets the delegate that is used to normalize string values.
    /// The default value is null. If no delegate is set, the default normalization
    /// function <see cref="StringExtensions.NormalizeString" /> will be used which
    /// converts null to empty strings and trims the value.
    /// </summary>
    public Func<string?, string>? NormalizeStringValue { get; set; }
}