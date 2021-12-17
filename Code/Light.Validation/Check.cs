namespace Light.Validation;

public readonly record struct Check<T>(ValidationContext Context, string Key, T Value)
{
    public void AddError(string errorMessage, bool tryAppend = true) =>
        Context.AddError(Key, errorMessage, tryAppend);
}