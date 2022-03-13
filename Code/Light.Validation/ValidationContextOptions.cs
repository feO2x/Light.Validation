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
    /// <param name="attachedObjects">The dictionary that will be used as the internal storage for attached objects.</param>
    /// <param name="disallowSettingAttachedObjects">
    /// The value indicating whether <see cref="ExtensibleObject.SetAttachedObject" /> will throw an exception when being called.
    /// If this value is set to true, the extensible object is immutable and the fully-filled dictionary of attached objects
    /// must be passed as a parameter to the constructor. Using this feature makes instances of this class thread-safe.
    /// </param>
    public ValidationContextOptions(Dictionary<string, object>? attachedObjects = null,
                                    bool disallowSettingAttachedObjects = false)
        : base(attachedObjects, disallowSettingAttachedObjects) { }

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
    /// is called. The default value is true.
    /// </summary>
    public bool NormalizeKeyOnRemoveError { get; init; } = true;

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

    /// <summary>
    /// Gets or sets the value indicating whether string values will be normalized
    /// when <see cref="ValidationContext.Check{T}" /> is called. The default value is true.
    /// </summary>
    public bool IsNormalizingStringValues { get; init; } = true;

    /// <summary>
    /// Gets or sets the delegate that is used to normalize string values.
    /// The default value is null. If no delegate is set, the default normalization
    /// function <see cref="StringExtensions.NormalizeString" /> will be used which
    /// converts null to empty strings and trims the value.
    /// </summary>
    public Func<string?, string>? NormalizeStringValue { get; init; }
}