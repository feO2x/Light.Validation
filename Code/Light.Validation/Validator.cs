using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Light.GuardClauses;

namespace Light.Validation;

/// <summary>
/// Represents the base class for validators that validate a single value which is usually a
/// Data Transfer Object (DTO). This validator runs synchronously.
/// </summary>
/// <typeparam name="T">The type of the value to be checked.</typeparam>
public abstract class Validator<T> : BaseValidator<T>
{
    /// <summary>
    /// Initializes a new instance of <see cref="Validator{T}" />.
    /// </summary>
    /// <param name="createValidationContext">
    /// The delegate that is used to create a new <see cref="ValidationContext" /> instance (optional).
    /// If null is provided, the constructor of <see cref="ValidationContext" /> is called with all parameters
    /// set to the default values.
    /// </param>
    /// <param name="isNullCheckingEnabled">
    /// The value indicating whether the validator automatically performs null-checking (optional).
    /// The default value is true. If enabled, the validator will automatically check if a value is
    /// null and then return the a error message that the value must not be null.
    /// </param>
    protected Validator(Func<ValidationContext>? createValidationContext = null,
                        bool isNullCheckingEnabled = true)
        : base(createValidationContext, isNullCheckingEnabled) { }

    /// <summary>
    /// Validates the specified value. If it has errors, true will be returned and the dictionary
    /// describing the errors will be set to the out parameter.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="errors">The dictionary that will contain all errors.</param>
    /// <param name="key">
    /// The string that identifies the passed value (optional). You do not need to pass this value as
    /// it is automatically obtained by the expression that is passed to <paramref name="value" />.
    /// This value is used to
    /// </param>
    /// <returns>True if at least one error was found, else false.</returns>
    public bool CheckForErrors(T value,
                               [NotNullWhen(true)] out object? errors,
                               [CallerArgumentExpression("value")] string key = "") =>
        CheckForErrors(value, CreateContext(), out errors, key);

    /// <summary>
    /// Validates the specified value while performing error tracking with the specified context.
    /// If it has errors, true will be returned and the dictionary
    /// describing the errors will be set to the out parameter.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="context">The validation context that manages the errors dictionary.</param>
    /// <param name="errors">The dictionary that will contain all errors.</param>
    /// <param name="key">
    /// The string that identifies the passed value (optional). You do not need to pass this value as
    /// it is automatically obtained by the expression that is passed to <paramref name="value" />.
    /// This value is used to
    /// </param>
    /// <returns>True if at least one error was found, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context" /> is null.</exception>
    public bool CheckForErrors(T value,
                               ValidationContext context,
                               [NotNullWhen(true)] out object? errors,
                               [CallerArgumentExpression("value")] string key = "")
    {
        context.MustNotBeNull();

        if (CheckForNull(value, context, key, out errors))
            return true;

        PerformValidation(context, value);
        if (context.TryGetErrors(out var errorsDictionary))
        {
            errors = errorsDictionary;
            return true;
        }

        errors = null;
        return false;
    }

    /// <summary>
    /// Validates the specified value and returning a structure containing the validation results.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    public ValidationResult Validate(T value) => Validate(value, CreateContext());

    /// <summary>
    /// Validates the specified value while performing error tracking with the specified context.
    /// This method returns a validation result.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="context">The validation context that manages the errors dictionary.</param>
    /// <param name="key">
    /// The string that identifies the passed value (optional). You do not need to pass this value as
    /// it is automatically obtained by the expression that is passed to <paramref name="value" />.
    /// This value is used to
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context" /> is null.</exception>
    public ValidationResult Validate(T value, ValidationContext context, [CallerArgumentExpression("value")] string key = "")
    {
        context.MustNotBeNull();

        if (CheckForNull(value, context, key, out var error))
            return new ValidationResult(error);

        PerformValidation(context, value);
        return context.CreateResult();
    }

    /// <summary>
    /// Performs the actual checks to validate the value. You should
    /// usually call <c>context.Check(value.SomeProperty)</c> on properties of the specified value
    /// to validate them. By default, the passed <paramref name="value" /> is not null because
    /// the validator performs an automatic null-check before calling this method. You can
    /// change this behavior by setting isNullCheckingEnabled to false when calling the base
    /// class constructor.
    /// </summary>
    /// <param name="context">The context that tracks errors in a dictionary.</param>
    /// <param name="value">The value to be checked.</param>
    protected abstract void PerformValidation(ValidationContext context, T value);
}