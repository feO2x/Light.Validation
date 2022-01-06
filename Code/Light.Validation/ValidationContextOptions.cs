using System;
using System.Collections.Generic;
using Light.Validation.Tools;

namespace Light.Validation;

/// <summary>
/// Represents options for the <see cref="ValidationContext" /> class.
/// </summary>
public record ValidationContextOptions
{
    /// <summary>
    /// Gets the default validation context options.
    /// </summary>
    public static ValidationContextOptions Default { get; } = new ();

    /// <summary>
    /// Gets or sets the value indicating whether the key
    /// is normalized when any of the ValidationContext.AddError
    /// methods is called. The default value is false.
    /// </summary>
    public bool NormalizeKeyOnAddError { get; init; } = false;

    /// <summary>
    /// Gets or sets the value indicating whether the key
    /// should be normalized when <see cref="ValidationContext.RemoveError" />
    /// is called. The default value is false.
    /// </summary>
    public bool NormalizeKeyOnRemoveError { get; init; } = false;

    /// <summary>
    /// Gets or sets the value indicating whether the key
    /// is normalized when <see cref="ValidationContext.Check{T}" />
    /// is called. The default value is true.
    /// </summary>
    public bool NormalizeKeyOnCheck { get; init; } = true;

    /// <summary>
    /// Gets or sets the delegate that is used to normalize the
    /// key. The default value is null. If no delegate is set,
    /// the default normalization function will be used which is
    /// <see cref="StringExtensions.NormalizeLastSectionToLowerCamelCase" />.
    /// </summary>
    public Func<string, string>? NormalizeKey { get; init; }
    
    /// <summary>
    /// Gets or sets the comparer that is used to compare keys in
    /// the errors dictionary. The default value is null, thus the
    /// EqualityComparer&lt;string&gt;.Default is used by the internal
    /// errors dictionary, comparing strings with ordinal options.
    /// </summary>
    public IEqualityComparer<string>? KeyComparer { get; init; } 

    /// <summary>
    /// Gets or sets the value indicating how multiple errors per key are
    /// handled. The default value is "AppendWithNewLine".
    /// </summary>
    public MultipleErrorsPerKeyBehavior MultipleErrorsPerKeyBehavior { get; init; } =
        MultipleErrorsPerKeyBehavior.AppendWithNewLine;

    /// <summary>
    /// Gets the string that is used as new line characters when combining error messages
    /// with the "AppendWithNewLine" behavior. The default value is "\n".
    /// </summary>
    public string NewLine { get; init; } = "\n";
}