namespace Light.Validation;

public readonly record struct Check<T>(ValidationContext Context, string Key, T Value)
{
    public void AddError(string errorMessage) =>
        Context.AddError(Key, errorMessage);

    public bool HasErrors => Context.Errors?.ContainsKey(Key) ?? false;

    public Check<T> WithNewValue(T newValue) =>
        new Check<T>(Context, Key, newValue);
}