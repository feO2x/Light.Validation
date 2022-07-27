using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Light.GuardClauses;

namespace Light.Validation;

/// <summary>
/// Represents the result of a validation run.
/// </summary>
/// <param name="ValidatedValue">
/// The value that was validated. This value might not be the original value that was received
/// as normalization might occur before validation.
/// </param>
/// <param name="Errors">The dictionary that contains all errors of the validation run.</param>
public readonly record struct ValidationResult<T>(T ValidatedValue, object? Errors)
{
    /// <summary>
    /// Gets the value indicating if <see cref="Errors" /> is null or contains no entries.
    /// This property will always return the inverse Boolean value of <see cref="HasErrors" />.
    /// </summary>
    public bool IsValid =>
        Errors switch
        {
            string errorMessage => errorMessage.IsNullOrWhiteSpace(),
            Dictionary<string, object> { Count: > 0 } => false,
            IList { Count: > 0 } => false,
            _ => Errors is null
        };

    /// <summary>
    /// Gets the value indicating if the errors dictionary has at least one entry.
    /// This property will always return the inverse Boolean value of <see cref="IsValid" />.
    /// </summary>
    public bool HasErrors => !IsValid;

    /// <summary>
    /// Tries to get the errors dictionary. The out parameter will only be set
    /// when the errors dictionary has at least one entry.
    /// </summary>
    /// <returns>True if the errors dictionary has at least one entry, else false.</returns>
    public bool TryGetErrors([NotNullWhen(true)] out object? errors)
    {
        if (IsValid)
        {
            errors = default;
            return false;
        }

        errors = Errors!;
        return true;
    }
}