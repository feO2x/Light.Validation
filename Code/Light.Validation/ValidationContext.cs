using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Validation;

public sealed class ValidationContext
{
    public Dictionary<string, object>? Errors { get; private set; }

    public bool HasErrors => Errors is { Count: > 0 };

    public void AddError(string key, string errorMessage, bool tryAppend = true)
    {
        key.MustNotBeNullOrWhiteSpace();
        
        if (Errors == null)
        {
            Errors = CreateErrorsDictionary();
            Errors.Add(key, errorMessage);
            return;
        }

        if (Errors.TryGetValue(key, out var existingError))
        {
            if (tryAppend)
            {
                if (existingError is not string existingErrorMessage)
                    throw new InvalidOperationException($"Cannot append error message to key \"{key}\" when its value is not a string.");
                errorMessage = existingErrorMessage + "\n" + errorMessage;
            }

            Errors[key] = errorMessage;
        }
        else
        {
            Errors.Add(key, errorMessage);
        }
    }

    public void AddError(string key, Dictionary<string, object> complexError)
    {
        key.MustNotBeNullOrWhiteSpace();

        if (Errors == null)
        {
            Errors = CreateErrorsDictionary();
            Errors.Add(key, complexError);
            return;
        }

        if (Errors.ContainsKey(key))
            Errors[key] = complexError;
        else
            Errors.Add(key, complexError);
    }

    public bool RemoveError(string key) => Errors?.Remove(key) ?? false;

    private static Dictionary<string, object> CreateErrorsDictionary() =>
        new (StringComparer.OrdinalIgnoreCase);
}