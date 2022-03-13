using System;
using System.Threading.Tasks;
using Light.GuardClauses;

namespace Light.Validation;

/// <summary>
/// Represents the base class for validators that validate a single value which is usually a
/// Data Transfer Object (DTO). This validator runs asynchronously.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public abstract class AsyncValidator<T>
{
    /// <summary>
    /// Initializes a new instance of <see cref="Validator{T}" />.
    /// </summary>
    /// <param name="createValidationContext">
    /// The delegate that is used to create a new <see cref="ValidationContext" /> instance (optional).
    /// If null is provided, the constructor of <see cref="ValidationContext" /> is called by default.
    /// </param>
    protected AsyncValidator(Func<ValidationContext>? createValidationContext = null) =>
        CreateValidationContext = createValidationContext;

    private Func<ValidationContext>? CreateValidationContext { get; }

    /// <summary>
    /// Asynchronously validates the specified value and returns a structure containing
    /// the validation result.
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    public Task<ValidationResult> ValidateAsync(T value) => ValidateAsync(value, CreateContext());

    /// <summary>
    /// Asynchronously validates the specified value while reusing the specified validation context.
    /// </summary>
    /// <param name="value">The value to be checked</param>
    /// <param name="context">The validation context that manages the errors dictionary.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
    public async Task<ValidationResult> ValidateAsync(T value, ValidationContext context)
    {
        context.MustNotBeNull();
        await PerformValidationAsync(context, value).ConfigureAwait(false);
        return context.CreateResult();
    }

    /// <summary>
    /// Performs the actual checks to validate the value. You should
    /// usually call context.Check on properties of the specified value
    /// to validate them.
    /// </summary>
    /// <param name="context">The context that tracks errors in a dictionary.</param>
    /// <param name="value">The value to be checked.</param>
    protected abstract Task PerformValidationAsync(ValidationContext context, T value);

    private ValidationContext CreateContext() => CreateValidationContext?.Invoke() ?? new ();
}