namespace Light.Validation;

public static class Checks
{
    public static Check<T> IsNotNull<T>(this Check<T?> check)
        where T : class
    {
        if (check.Value is null)
            check.AddError($"{check.Key} must not be null.");
        return check!;
    }
}