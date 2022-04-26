namespace Light.Validation.Tools;

/// <summary>
/// Provides extension methods for the <see cref="Check{T}" /> structure.
/// </summary>
public static class CheckExtensions
{
    /// <summary>
    /// Uses the information of the check instance to create a child validation context.
    /// </summary>
    public static ValidationContext CreateChildContext<T>(this Check<T> check) =>
        check.Context.Factory.CreateChildValidationContext(check.Context, check.Value);
}