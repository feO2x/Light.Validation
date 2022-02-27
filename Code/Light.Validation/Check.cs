using System;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation;

/// <summary>
/// Represents a structure that holds all necessary
/// objects to validate a value.
/// </summary>
/// <typeparam name="T">The type of the value to be checked.</typeparam>
public readonly record struct Check<T> : ICheck
{
    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" />.
    /// </summary>
    /// <param name="context">The context that manages the errors dictionary.</param>
    /// <param name="key">The key that identifies errors for the value.</param>
    /// <param name="value">The value to be checked.</param>
    /// <param name="isShortCircuited">
    /// The value indicating whether no further checks should
    /// be performed on this instance.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="context" /> or <paramref name="key" /> is null.
    /// </exception>
    public Check(ValidationContext context,
                 string key,
                 T value,
                 bool isShortCircuited = false)
    {
        Context = context.MustNotBeNull();
        Key = key.MustNotBeNull();
        Value = value;
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
    /// Gets the value to be checked.
    /// </summary>
    public T Value { get; }
    
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
    /// Adds the specified error message to the context. By default,
    /// the error will not be added when <see cref="IsShortCircuited" /> is true
    /// on this check instance. You can change this behavior by setting
    /// <paramref name="isRespectingShortCircuit" /> to false.
    /// </summary>
    /// <param name="errorMessage">The message that should be added to the context.</param>
    /// <param name="isRespectingShortCircuit">
    /// When this value is set to true (which is the default value),
    /// the error message will only be added to the context
    /// when <see cref="IsShortCircuited" /> is false. If this value is set
    /// to false, the error message will always be added.
    /// </param>
    public void AddError(string errorMessage, bool isRespectingShortCircuit = true)
    {
        if (!isRespectingShortCircuit || !IsShortCircuited)
            Context.AddError(Key, errorMessage);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Check{T}" /> with the
    /// same context and key, but with the specified value.
    /// </summary>
    public Check<T> WithNewValue(T newValue) => new (Context, Key, newValue);

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
    public Check<T> ShortCircuitIfNecessary(bool isShortCircuited) => new (Context, Key, Value, isShortCircuited);

    /// <summary>
    /// Gets the value indicating whether <see cref="Value" /> is null.
    /// </summary>
    public bool IsValueNull => Value is null;
}