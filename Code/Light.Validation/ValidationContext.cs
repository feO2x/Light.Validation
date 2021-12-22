using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation;

public sealed class ValidationContext
{
    public ValidationContext(Func<string, string>? normalizeKey = null) =>
        NormalizeKey = normalizeKey;

    public Dictionary<string, object>? Errors { get; private set; }

    private Func<string, string>? NormalizeKey { get; }

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
        new(StringComparer.OrdinalIgnoreCase);

    public Check<T> Check<T>(T value, [CallerArgumentExpression("value")] string key = "")
    {
        key.MustNotBeNullOrWhiteSpace();
        key = NormalizeKeyInternal(key);

        return new Check<T>(this, key, value);
    }

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

    private string NormalizeKeyInternal(string key) =>
        NormalizeKey?.Invoke(key) ?? key.NormalizeLastSectionToLowerCamelCase();

    public override string ToString()
    {
        if (Errors is null or { Count: 0 })
            return "No Errors";

        var stringBuilder = new StringBuilder();
        CreateStringRepresentationRecursively(stringBuilder, Errors);
        return stringBuilder.ToString();

        static void CreateStringRepresentationRecursively(StringBuilder stringBuilder, Dictionary<string, object> currentErrors)
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