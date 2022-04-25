namespace Light.Validation;

/// <summary>
/// Represents the abstraction of a factory that creates validation contexts.
/// </summary>
public interface IValidationContextFactory
{
    /// <summary>
    /// Creates a new validation context.
    /// </summary>
    ValidationContext CreateValidationContext();

    /// <summary>
    /// Creates a new validation context, usually used to validate
    /// a complex child value of a DTO (e.g. a collection or a complex child DTO).
    /// </summary>
    /// <typeparam name="T">The type of the child value.</typeparam>
    /// <param name="parent">The parent validation context.</param>
    /// <param name="childValue">The child value that requires a new validation context.</param>
    ValidationContext CreateChildValidationContext<T>(ValidationContext parent, T childValue);
}