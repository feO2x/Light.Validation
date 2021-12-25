using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Light.GuardClauses;

namespace Light.Validation;

/// <summary>
/// Represents the base class for validators that validate a single value which is usually a
/// Data Transfer Object (DTO). This validator runs synchronously.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public abstract class Validator<T>
{
    /// <summary>
    /// Validates the specified value. If it has errors, true will be returned and the dictionary
    /// describing the errors will be set to the out parameter.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="errors">The dictionary that will contain all errors.</param>
    /// <returns>True if at least one error was found, else false.</returns>
    public bool CheckForErrors(T value, [NotNullWhen(true)] out Dictionary<string, object>? errors) =>
        CheckForErrors(value, new ValidationContext(), out errors);

    /// <summary>
    /// Validates the specified value while performing error tracking with the specified context.
    /// If it has errors, true will be returned and the dictionary
    /// describing the errors will be set to the out parameter.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="context">The validation context that manages the errors dictionary.</param>
    /// <param name="errors">The dictionary that will contain all errors.</param>
    /// <returns>True if at least one error was found, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public bool CheckForErrors(T value,
                               ValidationContext context,
                               [NotNullWhen(true)] out Dictionary<string, object>? errors)
    {
        context.MustNotBeNull();
        PerformValidation(context, value);
        return context.TryGetErrors(out errors);
    }

    /// <summary>
    /// Validates the specified value and returning a structure containing the validation results.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    public ValidationResult Validate(T value) => Validate(value, new ValidationContext());

    /// <summary>
    /// Validates the specified value while performing error tracking with the specified context.
    /// This method returns a validation result.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="context">The validation context that manages the errors dictionary.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public ValidationResult Validate(T value, ValidationContext context)
    {
        context.MustNotBeNull();
        PerformValidation(context, value);
        return context.CreateResult();
    }

    /// <summary>
    /// Performs the actual checks to validate the value. You should
    /// usually call context.Check on properties of the specified value
    /// to validate them.
    /// </summary>
    /// <param name="context">The context that tracks errors in a dictionary.</param>
    /// <param name="value">The value to be checked.</param>
    protected abstract void PerformValidation(ValidationContext context, T value);
}