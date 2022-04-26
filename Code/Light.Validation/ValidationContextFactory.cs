using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation;

/// <summary>
/// Represents the default factory for creating validation contexts
/// </summary>
public sealed class ValidationContextFactory : IValidationContextFactory
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValidationContextFactory" />.
    /// </summary>
    /// <param name="options">The options that will be passed to the validation context.</param>
    /// <param name="errorTemplates">The error templates that will be passed to the validation context.</param>
    /// <param name="attachedObjects">The objects that will be attached to the validation context (optional).</param>
    public ValidationContextFactory(ValidationContextOptions options,
                                    ErrorTemplates errorTemplates,
                                    ExtensibleObject? attachedObjects = null)
    {
        Options = options.MustNotBeNull();
        ErrorTemplates = errorTemplates.MustNotBeNull();
        AttachedObjects = attachedObjects;
    }

    /// <summary>
    /// Gets the default instance of the validation context factory.
    /// </summary>
    public static ValidationContextFactory Instance { get; } =
        new (ValidationContextOptions.Default, ErrorTemplates.Default);

    private ValidationContextOptions Options { get; }
    private ErrorTemplates ErrorTemplates { get; }
    private  ExtensibleObject? AttachedObjects { get; }

    /// <summary>
    /// Creates the default validation context.
    /// </summary>
    public static ValidationContext CreateDefaultContext() => Instance.CreateValidationContext();

    /// <summary>
    /// Creates a new validation context, passing in the options,
    /// error templates, and attached objects that were specified in
    /// the constructor of this factory.
    /// </summary>
    public ValidationContext CreateValidationContext() =>
        new (this, Options, ErrorTemplates, AttachedObjects);

    /// <summary>
    /// Creates a new validation context the same way as
    /// <see cref="CreateValidationContext" />.
    /// </summary>
    public ValidationContext CreateChildValidationContext<T>(ValidationContext parent, T childValue) =>
        CreateValidationContext();
}