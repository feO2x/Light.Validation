﻿using System;
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
public class ValidationContext : ExtensibleObject
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValidationContext" />.
    /// </summary>
    /// <param name="factory">The factory that was used to create this context.</param>
    /// <param name="options">
    /// The options for this context (optional). If null is specified,
    /// <see cref="ValidationContextOptions.Default" /> will be used.
    /// </param>
    /// <param name="errorTemplates">
    /// The error templates for this context (optional). These are
    /// used to format the error messages if a check fails. If null
    /// is specified, <see cref="Tools.ErrorTemplates.Default" /> will be used.
    /// </param>
    /// <param name="other">Another extensible object whose attached objects will be shallow-copied to this instance.</param>
    public ValidationContext(IValidationContextFactory factory,
                             ValidationContextOptions options,
                             ErrorTemplates errorTemplates,
                             ExtensibleObject? other = null)
        : base(other)
    {
        Factory = factory.MustNotBeNull();
        Options = options.MustNotBeNull();
        ErrorTemplates = errorTemplates.MustNotBeNull();
    }

    /// <summary>
    /// Gets the factory that was used to create this context.
    /// </summary>
    public IValidationContextFactory Factory { get; }

    /// <summary>
    /// Gets the errors dictionary. This value can be null when no errors were
    /// recorded yet.
    /// </summary>
    public Dictionary<string, object>? Errors { get; private set; }

    /// <summary>
    /// Gets the options for this validation context.
    /// </summary>
    public ValidationContextOptions Options { get; }

    /// <summary>
    /// Gets the error templates for this context. These are
    /// used to format the error messages if a check fails.
    /// </summary>
    public ErrorTemplates ErrorTemplates { get; }

    /// <summary>
    /// Checks if the internal errors dictionary is initialized
    /// and contains at least one entry.
    /// </summary>
    public bool HasErrors => Errors is { Count: > 0 };

    /// <summary>
    /// <para>
    /// Adds an error message to the errors dictionary using the specified key.
    /// The key is not normalized when calling this method.
    /// </para>
    /// <para>
    /// When there already is an error message associated with the given key, your new
    /// error message will be appended with new line to the existing error message.
    /// You can change this behavior by passing in <see cref="Options" /> with
    /// a different value for MultipleErrorsPerKeyBehavior. If the given error is not
    /// a string and there already is an error for the specified key, it will be replaced.
    /// </para>
    /// </summary>
    /// <param name="key">The key that identifies the error.</param>
    /// <param name="error">The error that should be stored in the internal dictionary.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="key" /> or <paramref name="error" /> is null.
    /// </exception>
    public void AddError(string key, object error)
    {
        key.MustNotBeNull();
        error.MustNotBeNull();

        if (TryAddFirstError(key, error))
            return;

        if (error is string errorMessage)
            InsertOrUpdateErrorMessage(key, errorMessage);
        else
            Errors![key] = error;
    }

    /// <summary>
    /// Casts the options of this validation context to the specified subtype
    /// or throws an <see cref="InvalidCastException" />.
    /// </summary>
    /// <typeparam name="T">The subtype the options should be cast into.</typeparam>
    /// <exception cref="InvalidCastException">Thrown when <see cref="Options" /> cannot be cast to type T.</exception>
    public T GetOptionsAs<T>() where T : ValidationContextOptions
    {
        if (Options is T castOptions)
            return castOptions;

        throw new InvalidCastException($"The options cannot be cast to type \"{typeof(T)}\"");
    }

    /// <summary>
    /// Casts the error templates of this validation context to the specified subtype
    /// or throws an <see cref="InvalidCastException" />.
    /// </summary>
    /// <typeparam name="T">The subtype the error templates should be cast into.</typeparam>
    /// <exception cref="InvalidCastException">Thrown when <see cref="Options" /> cannot be cast to type T.</exception>
    public T GetErrorTemplatesAs<T>() where T : ErrorTemplates
    {
        if (ErrorTemplates is T castErrorTemplates)
            return castErrorTemplates;

        throw new InvalidCastException($"The error templates cannot be cast to type \"{typeof(T)}\"");
    }

    /// <summary>
    /// Removes the error with the specified key. The key is not normalized when calling this method.
    /// </summary>
    /// <param name="key">The key that identifies the error.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="key" /> is null.</exception>
    public bool RemoveError(string key) =>
        Errors?.Remove(key) ?? false;

    private Dictionary<string, object> CreateErrorsDictionary() => new (Options.KeyComparer);

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
    /// NormalizeKeyOnCheck set to false or setting the NormalizeKey delegate, or by passing in a dedicated
    /// value for the optional <paramref name="key" /> parameter. The default normalization algorithm will
    /// take the substring after the last dot and convert it to lowerCamelCase if necessary.
    /// </para>
    /// <para>
    /// When a string value is passed, this method will by default normalize it: null will
    /// be replaced with an empty string, non-null strings will be trimmed. You can change
    /// this behavior by passing in <see cref="Options" /> with IsNormalizingStringValues set
    /// to false, or by providing a delegate that performs your custom normalization (NormalizeStringValue).
    /// </para>
    /// </summary>
    /// <param name="value">The value to be checked.</param>
    /// <param name="normalizeValue">
    /// The value indicating whether strings should be normalized. The default value is null.
    /// If you set this value to true or false, it will take precedence over
    /// <see cref="ValidationContextOptions.IsNormalizingStringValues" /> (which is true by default).
    /// If the passed in value is no string, setting this parameter has no effect.
    /// </param>
    /// <param name="key">
    /// The string that identifies the corresponding errors in the internal dictionary of the validation context (optional).
    /// You do not need to pass this value as it is automatically obtained by the expression that is passed to <paramref name="value" />
    /// via the <see cref="CallerArgumentExpressionAttribute" />.
    /// </param>
    /// <param name="displayName">
    /// The human-readable name of the value (optional). The default value is null.
    /// This parameter will be set to the value of the <paramref name="key" /> parameter if null is passed.
    /// It will also be normalized if no dedicated display name is set.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public Check<T> Check<T>(T value,
                             bool? normalizeValue = null,
                             [CallerArgumentExpression("value")] string key = "",
                             string? displayName = null)
    {
        if (typeof(T) == typeof(string))
        {
            key.MustNotBeNull();

            displayName ??= key;

            if (!DetermineBooleanSetting(normalizeValue, Options.IsNormalizingStringValues))
                return new Check<T>(this, key, false, value, key);

            var stringValue = Unsafe.As<T, string>(ref value);
            stringValue = NormalizeStringValue(stringValue);
            return new Check<T>(this, key, false, Unsafe.As<string, T>(ref stringValue), displayName);
        }
        else
        {
            key.MustNotBeNull();

            displayName ??= key;

            if (value is string stringValue && DetermineBooleanSetting(normalizeValue, Options.IsNormalizingStringValues))
            {
                stringValue = NormalizeStringValue(stringValue);
                value = Unsafe.As<string, T>(ref stringValue);
            }

            return new Check<T>(this, key, false, value, displayName);
        }
    }

    private static bool DetermineBooleanSetting(bool? methodParameter, bool optionValue) => methodParameter ?? optionValue;

    /// <summary>
    /// Normalizes the specified string value.
    /// </summary>
    public string NormalizeStringValue(string? stringValue) =>
        Options.NormalizeStringValue?.Invoke(stringValue) ?? stringValue.NormalizeString();

    /// <summary>
    /// Tries to retrieve the errors that were tracked by this context.
    /// </summary>
    /// <param name="errors">The dictionary that contains the errors.</param>
    /// <returns>True if errors were added to this context, else false.</returns>
    public bool TryGetErrors([NotNullWhen(true)] out object? errors)
    {
        if (Errors is { Count: > 0 })
        {
            errors = Errors;
            return true;
        }

        errors = default;
        return false;
    }

    /// <summary>
    /// Normalizes the specified key.
    /// </summary>
    public string NormalizeKey(string key) =>
        Options.NormalizeKey?.Invoke(key) ?? key.GetSectionAfterLastDot();

    private bool TryAddFirstError(string key, object error)
    {
        if (Errors != null)
            return false;

        Errors = CreateErrorsDictionary();
        Errors.Add(key, error);
        return true;
    }

    private void InsertOrUpdateErrorMessage(string key,
                                            string errorMessage)
    {
        if (!Errors!.TryGetValue(key, out var existingError))
        {
            Errors.Add(key, errorMessage);
            return;
        }

        switch (existingError)
        {
            case string singleExistingError:
                Errors[key] = TransformSingleError(singleExistingError, errorMessage);
                break;
            case List<string> errorList:
                errorList.Add(errorMessage);
                break;
        }
    }

    /// <summary>
    /// Creates the error for automatic null checks. This method is called from validators as well as
    /// ValidateItems and ValidateItemsAsync when they are configured to perform automatic null checks
    /// and found a violation (i.e. the value is null). The default implementation will use the
    /// Not-Null error template to create the error message. You can customize
    /// </summary>
    /// <param name="key">The key of the error. The key is already normalized when this method is called.</param>
    /// <param name="displayName">The display name of the value.</param>
    public object CreateErrorForAutomaticNullCheck(string key, string displayName) =>
        Options.CreateErrorObjectForAutomaticNullCheck?.Invoke(this, key, displayName) ??
        string.Format(ErrorTemplates.NotNull, displayName);

    private object TransformSingleError(string singleExistingError,
                                        string errorMessage) =>
        Options.MultipleErrorsPerKeyBehavior switch
        {
            MultipleErrorsPerKeyBehavior.ReplaceError => errorMessage,
            MultipleErrorsPerKeyBehavior.PlaceInList => new List<string>(2) { singleExistingError, errorMessage },
            _ => singleExistingError + Options.NewLine + errorMessage
        };

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