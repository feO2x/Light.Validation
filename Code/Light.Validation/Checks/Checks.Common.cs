using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation.Checks;

/// <summary>
/// Provides extensions for the <see cref="Check{T}" /> structure
/// that allow easy validation of values.
/// </summary>
public static partial class Checks
{
    /// <summary>
    /// Checks if the specified value is not null, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when the value is null.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public static Check<T> IsNotNull<T>(this Check<T> check,
                                        string? message = null,
                                        bool shortCircuitOnError = true)
        where T : class
    {
        if (check.IsShortCircuited || !check.IsValueNull)
            return check;
        
        check.AddNotNullError(message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified value is not null, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when the value is null.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> IsNotNull<T>(this Check<T> check,
                                        Func<Check<T>, string> errorMessageFactory,
                                        bool shortCircuitOnError = true)
        where T : class
    {
        if (check.IsShortCircuited || !check.IsValueNull)
            return check;
        check.AddError(errorMessageFactory);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified nullable value is not null, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when the value is null.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public static Check<T?> IsNotNull<T>(this Check<T?> check,
                                         string? message = null,
                                         bool shortCircuitOnError = true)
        where T : struct
    {
        if (check.IsShortCircuited || !check.IsValueNull)
            return check;
        
        check.AddNotNullError(message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the specified value is not null, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when the value is null.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T?> IsNotNull<T>(this Check<T?> check,
                                         Func<Check<T?>, string> errorMessageFactory,
                                         bool shortCircuitOnError = true)
        where T : struct
    {
        if (check.IsShortCircuited || !check.IsValueNull)
            return check;
        check.AddError(errorMessageFactory);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is equal to the specified comparative value using the default equality comparer,
    /// or otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public static Check<T> IsEqualTo<T>(this Check<T> check, T comparativeValue, string? message = null)
    {
        if (!EqualityComparer<T>.Default.Equals(check.Value, comparativeValue))
            check.AddEqualToError(comparativeValue, message);
        return check;
    }

    /// <summary>
    /// Checks if the value is equal to the specified comparative value using the given equality comparer,
    /// or otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="equalityComparer">The equality comparer that is used to compare the two values.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
    public static Check<T> IsEqualTo<T>(this Check<T> check,
                                        T comparativeValue,
                                        IEqualityComparer<T> equalityComparer,
                                        string? message = null)
    {
        equalityComparer.MustNotBeNull();

        if (!equalityComparer.Equals(check.Value, comparativeValue))
            check.AddEqualToError(comparativeValue, message);
        return check;
    }

    /// <summary>
    /// Checks if the value is equal to the specified comparative value using the default equality comparer,
    /// or otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> IsEqualTo<T>(this Check<T> check,
                                        T comparativeValue,
                                        Func<Check<T>, T, string> errorMessageFactory)
    {
        if (!EqualityComparer<T>.Default.Equals(check.Value, comparativeValue))
            check.AddError(errorMessageFactory, comparativeValue);
        return check;
    }

    /// <summary>
    /// Checks if the value is equal to the specified comparative value using the given equality comparer,
    /// or otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="equalityComparer">The equality comparer that is used to compare the two values.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="equalityComparer" /> or <paramref name="errorMessageFactory" /> are null.
    /// </exception>
    public static Check<T> IsEqualTo<T>(this Check<T> check,
                                        T comparativeValue,
                                        IEqualityComparer<T> equalityComparer,
                                        Func<Check<T>, T, string> errorMessageFactory)
    {
        equalityComparer.MustNotBeNull();

        if (!equalityComparer.Equals(check.Value, comparativeValue))
            check.AddError(errorMessageFactory, comparativeValue);
        return check;
    }

    /// <summary>
    /// Checks if the value is not equal to the specified comparative value using the default equality comparer,
    /// or otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    public static Check<T> IsNotEqualTo<T>(this Check<T> check,
                                           T comparativeValue,
                                           string? message = null)
    {
        if (EqualityComparer<T>.Default.Equals(check.Value, comparativeValue))
            check.AddNotEqualToError(comparativeValue, message);
        return check;
    }

    /// <summary>
    /// Checks if the value is not equal to the specified comparative value using the given equality comparer,
    /// or otherwise adds an error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="equalityComparer">The equality comparer that is used to compare the two values.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="equalityComparer" /> is null.</exception>
    public static Check<T> IsNotEqualTo<T>(this Check<T> check,
                                           T comparativeValue,
                                           IEqualityComparer<T> equalityComparer,
                                           string? message = null)
    {
        equalityComparer.MustNotBeNull();

        if (equalityComparer.Equals(check.Value, comparativeValue))
            check.AddNotEqualToError(comparativeValue, message);
        return check;
    }

    /// <summary>
    /// Checks if the value is not equal to the specified comparative value using the default equality comparer,
    /// or otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> IsNotEqualTo<T>(this Check<T> check,
                                           T comparativeValue,
                                           Func<Check<T>, T, string> errorMessageFactory)
    {
        if (EqualityComparer<T>.Default.Equals(check.Value, comparativeValue))
            check.AddError(errorMessageFactory, comparativeValue);
        return check;
    }

    /// <summary>
    /// Checks if the value is not equal to the specified comparative value using the given equality comparer,
    /// or otherwise adds the error message that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The comparative value that is compared to the actual value.</param>
    /// <param name="equalityComparer">The equality comparer that is used to compare the two values.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="equalityComparer" /> or <paramref name="errorMessageFactory" /> are null.
    /// </exception>
    public static Check<T> IsNotEqualTo<T>(this Check<T> check,
                                           T comparativeValue,
                                           IEqualityComparer<T> equalityComparer,
                                           Func<Check<T>, T, string> errorMessageFactory)
    {
        equalityComparer.MustNotBeNull();

        if (equalityComparer.Equals(check.Value, comparativeValue))
            check.AddError(errorMessageFactory, comparativeValue);
        return check;
    }

    /// <summary>
    /// Checks if the specified GUID is not empty, or otherwise adds an error message to the
    /// validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    public static Check<Guid> IsNotEmpty(this Check<Guid> check, string? message = null)
    {
        if (check.Value == Guid.Empty)
            check.AddNotEmptyGuidError(message);
        return check;
    }

    /// <summary>
    /// Checks if the specified GUID is not empty, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<Guid> IsNotEmpty(this Check<Guid> check, Func<Check<Guid>, string> errorMessageFactory)
    {
        if (check.Value == Guid.Empty)
            check.AddError(errorMessageFactory);
        return check;
    }
}