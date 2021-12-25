using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation;

/// <summary>
/// Represents an object that tracks errors in
/// a dictionary. The dictionary will only be initialized when
/// errors are actually added to avoid Garbage Collector pressure.
/// </summary>
public sealed class ValidationContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValidationContext" />.
    /// </summary>
    /// <param name="options">
    /// The options for this context (optional). If null is specified,
    /// <see cref="ValidationContextOptions.Default" /> will be used.
    /// </param>
    public ValidationContext(ValidationContextOptions? options = null)
    {
        Options = options ?? ValidationContextOptions.Default;
    }

    /// <summary>
    /// Gets the errors dictionary. This value can be null when no errors were
    /// recorded yet.
    /// </summary>
    public Dictionary<string, object>? Errors { get; private set; }

    private ValidationContextOptions Options { get; }

    /// <summary>
    /// Checks if the internal errors dictionary is initialized
    /// and contains at least one entry.
    /// </summary>
    public bool HasErrors => Errors is { Count: > 0 };

    /// <summary>
    /// <para>
    /// Adds an error message to the errors dictionary using the specified key.
    /// </para>
    /// <para>
    /// By default, the key is not normalized when calling this method.
    /// You can change this by passing in <see cref="Options" /> with
    /// NormalizeKeyOnAddError set to true.
    /// </para>
    /// <para>
    /// When there already is an error associated with the given key, your new
    /// error message will be appended with new line to the existing error message.
    /// You can change this behavior by passing in <see cref="Options" /> with
    /// a different value for MultipleErrorsPerKeyBehavior.
    /// </para>
    /// </summary>
    /// <param name="key">The key that identifies the error.</param>
    /// <param name="errorMessage">The message associated with the error.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="key" /> or <paramref name="errorMessage" /> is null.
    /// </exception>
    public void AddError(string key, string errorMessage)
    {
        key.MustNotBeNull();
        errorMessage.MustNotBeNull();

        key = NormalizeKey(key, Options.NormalizeKeyOnAddError);

        if (TryAddFirstError(key, errorMessage))
            return;

        InsertOrUpdateError(key, errorMessage, Options);
    }

    /// <summary>
    /// <para>
    /// Adds the complex error to the errors dictionary using the specified key.
    /// </para>
    /// <para>
    /// By default, the key is not normalized when calling this method.
    /// You can change this by passing in <see cref="Options" /> with
    /// NormalizeKeyOnAddError set to true.
    /// </para>
    /// <para>
    /// When there already is an error for the specified key, it will be
    /// replaced.
    /// </para>
    /// </summary>
    /// <param name="key">The key that identifies the error.</param>
    /// <param name="complexError">The dictionary that describes the complex error.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="key" /> or <paramref name="complexError" /> is null.
    /// </exception>
    public void AddError(string key, Dictionary<string, object> complexError)
    {
        key.MustNotBeNull();
        complexError.MustNotBeNull();

        key = NormalizeKey(key, Options.NormalizeKeyOnAddError);

        if (TryAddFirstError(key, complexError))
            return;

        if (Errors!.ContainsKey(key))
            Errors[key] = complexError;
        else
            Errors.Add(key, complexError);
    }

    /// <summary>
    /// <para>
    /// Removes the error with the specified key.
    /// </para>
    /// <para>
    /// By default, the key is not normalized when calling this method.
    /// You can change this by passing in <see cref="Options" /> with
    /// NormalizeKeyOnRemoveError set to true.
    /// </para>
    /// </summary>
    /// <param name="key">The key that identifies the error.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key" /> is null.</exception>
    public bool RemoveError(string key)
    {
        key.MustNotBeNull();

        key = NormalizeKey(key, Options.NormalizeKeyOnRemoveError);
        return Errors?.Remove(key) ?? false;
    }

    private Dictionary<string, object> CreateErrorsDictionary()
    {
        return new Dictionary<string, object>(Options.KeyComparer);
    }

    /// <summary>
    /// <para>
    /// Creates a <see cref="Check{T}" /> instance that can be easily
    /// used to apply checks to the target value. Simply call the existing
    /// extension methods that can be found in the <see cref="Checks" />
    /// class or write your own extension methods.
    /// </para>
    /// <para>
    /// By default, the error key is automatically determined using the
    /// CallerArgumentExpressionAttribute and then normalized.
    /// You can change this by passing in <see cref="Options" /> with
    /// NormalizeKeyOnCheck set to false and passing in a dedicated
    /// value for the optional <paramref name="key" /> parameter.
    /// </para>
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="key">The key that will identify the error (optional).</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public Check<T> Check<T>(T value, [CallerArgumentExpression("value")] string key = "")
    {
        key.MustNotBeNull();
        key = NormalizeKey(key, Options.NormalizeKeyOnCheck);

        return new Check<T>(this, key, value);
    }

    /// <summary>
    /// Creates a validation result with the internal errors dictionary.
    /// </summary>
    public ValidationResult CreateResult() => new (Errors);

    /// <summary>
    /// Tries to retrieve the errors that were tracked by this context.
    /// </summary>
    /// <param name="errors">The dictionary that contains the errors.</param>
    /// <returns>True if errors were added to this context, else false.</returns>
    public bool TryGetErrors([NotNullWhen(true)] out Dictionary<string, object>? errors)
    {
        if (Errors is { Count: > 0 })
        {
            errors = Errors;
            return true;
        }

        errors = default;
        return false;
    }

    private string NormalizeKey(string key, bool condition)
    {
        if (!condition)
            return key;

        return Options.NormalizeKey?.Invoke(key) ?? key.NormalizeLastSectionToLowerCamelCase();
    }

    private bool TryAddFirstError(string key, object error)
    {
        if (Errors != null)
            return false;

        Errors = CreateErrorsDictionary();
        Errors.Add(key, error);
        return true;
    }

    private void InsertOrUpdateError(string key,
                                     string errorMessage,
                                     ValidationContextOptions options)
    {
        if (!Errors!.TryGetValue(key, out var existingError))
        {
            Errors.Add(key, errorMessage);
            return;
        }

        switch (existingError)
        {
            case string singleExistingError:
                Errors[key] = TransformSingleError(singleExistingError, errorMessage, options);
                break;
            case List<string> errorList:
                errorList.Add(errorMessage);
                break;
        }
    }

    private static object TransformSingleError(string singleExistingError,
                                               string errorMessage,
                                               ValidationContextOptions options)
    {
        return options.MultipleErrorsPerKeyBehavior switch
        {
            MultipleErrorsPerKeyBehavior.ReplaceError => errorMessage,
            MultipleErrorsPerKeyBehavior.PlaceInList => new List<string>(2) { singleExistingError, errorMessage },
            _ => singleExistingError + options.NewLine + errorMessage
        };
    }

    /// <summary>
    /// Returns the string representation of this validation context.
    /// </summary>
    public override string ToString()
    {
        if (Errors is null or { Count: 0 })
            return "No Errors";

        var stringBuilder = new StringBuilder();
        CreateStringRepresentationRecursively(stringBuilder, Errors);
        return stringBuilder.ToString();

        static void CreateStringRepresentationRecursively(StringBuilder stringBuilder,
                                                          Dictionary<string, object> currentErrors)
        {
            stringBuilder.Append("{ ");
            var count = currentErrors.Count;
            var i = 0;
            foreach (var keyValuePair in currentErrors)
            {
                stringBuilder.Append(keyValuePair.Key)
                   .Append(": ");
                switch (keyValuePair.Value)
                {
                    case string errorMessage:
                        stringBuilder.Append(errorMessage);
                        break;
                    case Dictionary<string, object> childErrors:
                        CreateStringRepresentationRecursively(stringBuilder, childErrors);
                        break;
                }

                if (i++ < count - 1)
                    stringBuilder.Append(", ");
            }

            stringBuilder.Append(" }");
        }
    }
}