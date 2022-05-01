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
    /// <param name="validationContextFactory">
    /// The factory that is used to create a new <see cref="ValidationContext" /> instance.
    /// </param>
    /// <param name="isNullCheckingEnabled">
    /// The value indicating whether the validator automatically performs null-checking (optional).
    /// The default value is true. If enabled, the validator will automatically check if a value is
    /// null and then return the default error message that the value must not be null.
    /// The default error message is created by <see cref="ValidationContext.CreateErrorForAutomaticNullCheck" />
    /// and can be overridden.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="validationContextFactory" /> is null.</exception>
    protected Validator(IValidationContextFactory validationContextFactory,
                        bool isNullCheckingEnabled = true)
        : base(validationContextFactory, isNullCheckingEnabled) { }

    /// <summary>
    /// Validates the specified value. If it has errors, true will be returned and the dictionary
    /// describing the errors will be set to the out parameter.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="errors">The dictionary that will contain all errors.</param>
    /// <param name="key">
    /// The string that identifies the corresponding errors in the internal dictionary of the validation context (optional).
    /// You do not need to pass this value as it is automatically obtained by the expression that is passed to <paramref name="value" />
    /// via the <see cref="CallerArgumentExpressionAttribute" />. This value is only relevant if this validator is
    /// called by another validator.
    /// </param>
    /// <param name="displayName">
    /// The human-readable name of the value (optional). The default value is null.
    /// This parameter will be set to the value of the <paramref name="key" /> parameter if null is passed.
    /// It will also be normalized if no dedicated display name is set.
    /// </param>
    /// <returns>True if at least one error was found, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
    public bool CheckForErrors([NotNullWhen(false)] T? value,
                               [NotNullWhen(true)] out object? errors,
                               [CallerArgumentExpression("value")] string key = "",
                               string? displayName = null) =>
        CheckForErrors(value, ValidationContextFactory.CreateValidationContext(), out errors, key, displayName);

    /// <summary>
    /// Validates the specified value while performing error tracking with the specified context.
    /// If it has errors, true will be returned and the dictionary
    /// describing the errors will be set to the out parameter.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="context">The validation context that manages the errors dictionary.</param>
    /// <param name="errors">The dictionary that will contain all errors.</param>
    /// <param name="key">
    /// The string that identifies the corresponding errors in the internal dictionary of the validation context (optional).
    /// You do not need to pass this value as it is automatically obtained by the expression that is passed to <paramref name="value" />
    /// via the <see cref="CallerArgumentExpressionAttribute" />. This value is only relevant if this validator is
    /// called by another validator.
    /// </param>
    /// <param name="displayName">
    /// The human-readable name of the value (optional). The default value is null.
    /// This parameter will be set to the value of the <paramref name="key" /> parameter if null is passed.
    /// It will also be normalized if no dedicated display name is set.
    /// </param>
    /// <returns>True if at least one error was found, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context" /> or <paramref name="key"/> are null.</exception>
    public bool CheckForErrors([NotNullWhen(false)] T? value,
                               ValidationContext context,
                               [NotNullWhen(true)] out object? errors,
                               [CallerArgumentExpression("value")] string key = "",
                               string? displayName = null)
    {
        context.MustNotBeNull();
        key.MustNotBeNull();

        displayName ??= key;

        if (TryCheckForNull(value, context, key, displayName, out errors))
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
    /// <param name="key">
    /// The string that identifies the corresponding errors in the internal dictionary of the validation context (optional).
    /// You do not need to pass this value as it is automatically obtained by the expression that is passed to <paramref name="value" />
    /// via the <see cref="CallerArgumentExpressionAttribute" />. This value is only relevant if this validator is
    /// called by another validator.
    /// </param>
    /// <param name="displayName">
    /// The human-readable name of the value (optional). The default value is null.
    /// This parameter will be set to the value of the <paramref name="key" /> parameter if null is passed.
    /// It will also be normalized if no dedicated display name is set.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
    public ValidationResult<T> Validate([ValidatedNotNull] T? value,
                                        [CallerArgumentExpression("value")] string key = "",
                                        string? displayName = null) =>
        Validate(value, ValidationContextFactory.CreateValidationContext(), key, displayName);

    /// <summary>
    /// Validates the specified value while performing error tracking with the specified context.
    /// This method returns a validation result.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="context">The validation context that manages the errors dictionary.</param>
    /// <param name="key">
    /// The string that identifies the corresponding errors in the internal dictionary of the validation context (optional).
    /// You do not need to pass this value as it is automatically obtained by the expression that is passed to <paramref name="value" />
    /// via the <see cref="CallerArgumentExpressionAttribute" />. This value is only relevant if this validator is
    /// called by another validator.
    /// </param>
    /// <param name="displayName">
    /// The human-readable name of the value (optional). The default value is null.
    /// This parameter will be set to the value of the <paramref name="key" /> parameter if null is passed.
    /// It will also be normalized if no dedicated display name is set.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context" /> or <paramref name="key"/> are null.</exception>
    public ValidationResult<T> Validate([ValidatedNotNull] T? value,
                                        ValidationContext context,
                                        [CallerArgumentExpression("value")] string key = "",
                                        string? displayName = null)
    {
        context.MustNotBeNull();
        key.MustNotBeNull();

        displayName ??= key;

        if (TryCheckForNull(value, context, key, displayName, out var error))
            return new ValidationResult<T>(value!, error);

        value = PerformValidation(context, value);
        return new ValidationResult<T>(value, context.Errors);
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
    protected abstract T PerformValidation(ValidationContext context, T value);
}