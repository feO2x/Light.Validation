using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Light.Validation;

public abstract class Validator<T>
{
    public bool CheckForErrors(T value, [NotNullWhen(true)] out Dictionary<string, object>? errors)
    {
        var context = new ValidationContext();
        CheckForErrors(context, value);
        return context.TryGetErrors(out errors);
    }

    protected abstract void CheckForErrors(ValidationContext context, T value);
}