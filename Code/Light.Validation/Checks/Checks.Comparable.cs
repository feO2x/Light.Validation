using System;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation.Checks;

public static partial class Checks
{
    /// <summary>
    /// Checks if the value is greater than the specified other value, or otherwise adds an
    /// error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparativeValue" /> is null.</exception>
    public static Check<T> IsGreaterThan<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsValueNull)
            return check;

        if (check.Value.CompareTo(comparativeValue) <= 0)
            check.AddGreaterThanError(comparativeValue, message);

        return check;
    }

    /// <summary>
    /// Checks if the value is greater than the specified other value, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="comparativeValue" /> or <paramref name="errorMessageFactory"/> is null.
    /// </exception>
    public static Check<T> IsGreaterThan<T>(this Check<T> check,
                                            T comparativeValue,
                                            Func<Check<T>, T, string> errorMessageFactory)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (!check.IsValueNull && check.Value.CompareTo(comparativeValue) <= 0)
            check.AddError(errorMessageFactory, comparativeValue);

        return check;
    }

    /// <summary>
    /// Checks if the value is greater than or equal to the specified other value, or otherwise adds an
    /// error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparativeValue" /> is null.</exception>
    public static Check<T> IsGreaterThanOrEqualTo<T>(this Check<T> check,
                                                     T comparativeValue,
                                                     string? message = null)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();
        
        if (!check.IsValueNull && check.Value.CompareTo(comparativeValue) < 0)
            check.AddGreaterThanOrEqualToError(comparativeValue, message);

        return check;
    }

    /// <summary>
    /// Checks if the value is greater than or equal to the specified other value, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="comparativeValue" /> or <paramref name="errorMessageFactory"/> is null.
    /// </exception>
    public static Check<T> IsGreaterThanOrEqualTo<T>(this Check<T> check,
                                                     T comparativeValue,
                                                     Func<Check<T>, T, string> errorMessageFactory)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();
        
        if (!check.IsValueNull && check.Value.CompareTo(comparativeValue) < 0)
            check.AddError(errorMessageFactory, comparativeValue);

        return check;
    }

    /// <summary>
    /// Checks if the value is less than the specified other value, or otherwise adds an
    /// error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparativeValue" /> is null.</exception>
    public static Check<T> IsLessThan<T>(this Check<T> check, T comparativeValue, string? message = null)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();
        
        if (!check.IsValueNull && check.Value.CompareTo(comparativeValue) >= 0)
            check.AddLessThanError(comparativeValue, message);

        return check;
    }

    /// <summary>
    /// Checks if the value is less than the specified other value, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="comparativeValue" /> or <paramref name="errorMessageFactory"/> is null.
    /// </exception>
    public static Check<T> IsLessThan<T>(this Check<T> check,
                                         T comparativeValue,
                                         Func<Check<T>, T, string> errorMessageFactory)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();
        
        if (!check.IsValueNull && check.Value.CompareTo(comparativeValue) >= 0)
            check.AddError(errorMessageFactory, comparativeValue);

        return check;
    }
}