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
    /// <param name="value">The value to be checked.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="context" /> or <paramref name="key" /> is null.
    /// </exception>
    public Check(ValidationContext context, string key, T value)
    {
        Context = context.MustNotBeNull();
        Key = key.MustNotBeNull();
        Value = value;
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
    /// Gets the value to be checked.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets the value indicating whether the context has errors for the key of this check instance.
    /// </summary>
    public bool HasError => Context.Errors?.ContainsKey(Key) ?? false;

    /// <summary>
    /// Adds the specified error message to the context.
    /// </summary>
    public void AddError(string errorMessage) => Context.AddError(Key, errorMessage);

    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" /> with the
    /// same context and key, but with the specified value.
    /// </summary>
    public Check<T> WithNewValue(T newValue) => new (Context, Key, newValue);
}