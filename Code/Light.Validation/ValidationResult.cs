using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Light.Validation;

/// <summary>
/// Represents the result of a validation run.
/// </summary>
/// <param name="Errors">The dictionary that contains all errors of the validation run.</param>
public readonly record struct ValidationResult(Dictionary<string, object>? Errors)
{
    /// <summary>
    /// Gets the value indicating if the errors dictionary is null or contains no entries.
    /// This property will always return the inverse Boolean value of <see cref="HasErrors" />.
    /// </summary>
    public bool IsValid => Errors is null or { Count: 0 };

    /// <summary>
    /// Gets the value indicating if the errors dictionary has at least one entry.
    /// This property will always return the inverse Boolean value of <see cref="IsValid" />.
    /// </summary>
    public bool HasErrors => Errors is { Count: > 0 };

    /// <summary>
    /// Tries to get the errors dictionary. The out parameter will only be set
    /// when the errors dictionary has at least one entry.
    /// </summary>
    /// <returns>True if the errors dictionary has at least one entry, else false.</returns>
    public bool TryGetErrors([NotNullWhen(true)] out Dictionary<string, object>? errors)
    {
        if (HasErrors)
        {
            errors = Errors!;
            return true;
        }

        errors = default;
        return false;
    }
}