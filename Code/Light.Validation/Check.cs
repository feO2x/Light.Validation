using System;
using Light.GuardClauses;

namespace Light.Validation;

/// <summary>
/// Represents a structure that holds all necessary
/// objects to validate a value.
/// </summary>
/// <typeparam name="T">The type of the value to be checked.</typeparam>
public readonly record struct Check<T>
{
    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" />.
    /// </summary>
    /// <param name="context">The context that manages the errors dictionary.</param>
    /// <param name="key">The key that identifies errors for the value.</param>
    /// <param name="isKeyNormalized">The value indicating whether the <paramref name="key" /> is normalized.</param>
    /// <param name="displayName">The human-readable name of the value.</param>
    /// <param name="value">The value to be checked.</param>
    /// <param name="isShortCircuited">
    /// The value indicating whether no further checks should
    /// be performed on this instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="context" />, <paramref name="key" />, or <paramref name="displayName" /> are null.
    /// </exception>
    public Check(ValidationContext context,
                 string key,
                 bool isKeyNormalized,
                 T value,
                 string displayName,
                 bool isShortCircuited = false)
    {
        Context = context.MustNotBeNull();
        Key = key.MustNotBeNull();
        IsKeyNormalized = isKeyNormalized;
        Value = value;
        DisplayName = displayName.MustNotBeNull();
        IsShortCircuited = isShortCircuited;
    }

    /// <summary>
    /// Gets the context that manages the errors dictionary.
    /// </summary>
    public ValidationContext Context { get; }

    /// <summary>
    /// Gets the key that identifies errors for the value.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the value indicating whether the key has already been normalized.
    /// </summary>
    public bool IsKeyNormalized { get; }

    /// <summary>
    /// Gets the value to be checked.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets the human-readable name of the value.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Gets the value indicating whether no further checks should
    /// be performed with this instance.
    /// </summary>
    public bool IsShortCircuited { get; }

    /// <summary>
    /// Gets the value indicating whether the context has errors for the key of this check instance.
    /// </summary>
    public bool HasError => Context.Errors?.ContainsKey(Key) ?? false;

    /// <summary>
    /// Gets the value indicating whether <see cref="Value" /> is null.
    /// </summary>
    public bool IsValueNull => Value is null;

    /// <summary>
    /// Adds the specified error message to the context. By default,
    /// the error will not be added when <see cref="IsShortCircuited" /> is true
    /// on this check instance. You can change this behavior by setting
    /// <paramref name="isRespectingShortCircuit" /> to false.
    /// </summary>
    /// <param name="error">The error that should be added to the validation context.</param>
    /// <param name="isRespectingShortCircuit">
    /// When this value is set to true (which is the default value),
    /// the error message will only be added to the context
    /// when <see cref="IsShortCircuited" /> is false. If this value is set
    /// to false, the error message will always be added.
    /// </param>
    public Check<T> AddError(object error, bool isRespectingShortCircuit = true)
    {
        if (!isRespectingShortCircuit || !IsShortCircuited)
            Context.AddError(Key, error);
        return this;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" /> with the specified value.
    /// All other values stay the same.
    /// </summary>
    public Check<T> WithNewValue(T newValue) => new (Context, Key, IsKeyNormalized, newValue, DisplayName, IsShortCircuited);

    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" /> with the specified display name.
    /// All other values stay the same.
    /// </summary>
    public Check<T> WithDisplayName(string displayName) => new (Context, Key, IsKeyNormalized, Value, displayName, IsShortCircuited);

    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" /> with the
    /// same context, key, and value, but with <see cref="IsShortCircuited" />
    /// set to true.
    /// </summary>
    public Check<T> ShortCircuit() => ShortCircuitIfNecessary(true);

    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" /> with the
    /// same context, key, and value, but with <see cref="IsShortCircuited" />
    /// set to the specified value.
    /// </summary>
    public Check<T> ShortCircuitIfNecessary(bool isShortCircuited) => new (Context, Key, IsKeyNormalized, Value, DisplayName, isShortCircuited);

    /// <summary>
    /// Casts the validation context to the specified subtype and returns
    /// it. If casting is not possible, an <see cref="InvalidCastException" />
    /// will be thrown.
    /// </summary>
    /// <typeparam name="TValidationContext">The subtype of the validation context.</typeparam>
    /// <exception cref="InvalidCastException">Thrown when <see cref="Context" /> cannot be cast to type TValidationContext.</exception>
    public TValidationContext GetContextAs<TValidationContext>()
        where TValidationContext : ValidationContext
    {
        if (Context is TValidationContext subContext)
            return subContext;

        throw new InvalidCastException($"The validation context cannot be cast to type \"{typeof(TValidationContext)}\".");
    }

    /// <summary>
    /// Normalizes the key if necessary. If the key is already normalized, the same instance will be
    /// returned, or otherwise a new instance with the normalized key will be returned.
    /// </summary>
    public Check<T> NormalizeKeyIfNecessary()
    {
        if (IsKeyNormalized || !Context.Options.IsNormalizingKeys)
            return this;

        var key = Context.NormalizeKey(Key);
        var displayName = DisplayName;
        if (ReferenceEquals(displayName, Key))
            displayName = key;
        return new Check<T>(Context, key, true, Value, displayName, IsShortCircuited);
    }

    /// <summary>
    /// Implicitly converts the check to its internal value.
    /// </summary>
    public static implicit operator T(Check<T> check) => check.Value;
}