﻿using System;
using Light.GuardClauses;
using Light.Validation.Tools;
using Range = Light.Validation.Tools.Range;

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
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparativeValue" /> is null.</exception>
    public static Check<T> IsGreaterThan<T>(this Check<T> check,
                                            T comparativeValue,
                                            string? message = null,
                                            bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) > 0)
            return check;

        check = check.AddGreaterThanError(comparativeValue, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is greater than the specified other value, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="comparativeValue" /> or <paramref name="errorMessageFactory" /> is null.
    /// </exception>
    public static Check<T> IsGreaterThan<T>(this Check<T> check,
                                            T comparativeValue,
                                            Func<Check<T>, T, string> errorMessageFactory,
                                            bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) > 0)
            return check;

        check = check.CreateAndAddError(errorMessageFactory, comparativeValue);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
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
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparativeValue" /> is null.</exception>
    public static Check<T> IsGreaterThanOrEqualTo<T>(this Check<T> check,
                                                     T comparativeValue,
                                                     string? message = null,
                                                     bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) >= 0)
            return check;

        check = check.AddGreaterThanOrEqualToError(comparativeValue, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is greater than or equal to the specified other value, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="comparativeValue" /> or <paramref name="errorMessageFactory" /> is null.
    /// </exception>
    public static Check<T> IsGreaterThanOrEqualTo<T>(this Check<T> check,
                                                     T comparativeValue,
                                                     Func<Check<T>, T, string> errorMessageFactory,
                                                     bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) >= 0)
            return check;

        check = check.CreateAndAddError(errorMessageFactory, comparativeValue);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
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
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparativeValue" /> is null.</exception>
    public static Check<T> IsLessThan<T>(this Check<T> check,
                                         T comparativeValue,
                                         string? message = null,
                                         bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) < 0)
            return check;

        check = check.AddLessThanError(comparativeValue, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is less than the specified other value, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="comparativeValue" /> or <paramref name="errorMessageFactory" /> is null.
    /// </exception>
    public static Check<T> IsLessThan<T>(this Check<T> check,
                                         T comparativeValue,
                                         Func<Check<T>, T, string> errorMessageFactory,
                                         bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) < 0)
            return check;

        check = check.CreateAndAddError(errorMessageFactory, comparativeValue);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is less than or equal to the specified other value, or otherwise adds an
    /// error message to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparativeValue" /> is null.</exception>
    public static Check<T> IsLessThanOrEqualTo<T>(this Check<T> check,
                                                  T comparativeValue,
                                                  string? message = null,
                                                  bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) <= 0)
            return check;

        check = check.AddLessThanOrEqualToError(comparativeValue, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is less than or equal to the specified other value, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="comparativeValue">The other value the actual value is compared against.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="comparativeValue" /> or <paramref name="errorMessageFactory" /> is null.
    /// </exception>
    public static Check<T> IsLessThanOrEqualTo<T>(this Check<T> check,
                                                  T comparativeValue,
                                                  Func<Check<T>, T, string> errorMessageFactory,
                                                  bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        comparativeValue.MustNotBeNullReference();

        if (check.IsShortCircuited || check.IsValueNull || check.Value.CompareTo(comparativeValue) <= 0)
            return check;

        check = check.CreateAndAddError(errorMessageFactory, comparativeValue);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is within the specified range, or otherwise adds an error message to the validation context.
    /// </summary>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">
    /// The value describing the allowed lower and upper boundaries.
    /// Use the static <see cref="Range" /> class to fluently create a <see cref="Range{T}" /> instance.
    /// </param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<T> IsIn<T>(this Check<T> check,
                                   Range<T> range,
                                   string? message = null,
                                   bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        if (check.IsShortCircuited || !check.IsValueNull && range.IsValueWithinRange(check.Value))
            return check;

        check = check.AddInRangeError(range, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is within the specified range, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">
    /// The value describing the allowed lower and upper boundaries.
    /// Use the static <see cref="Range" /> class to fluently create a <see cref="Range{T}" /> instance.
    /// </param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> IsIn<T>(this Check<T> check,
                                   Range<T> range,
                                   Func<Check<T>, Range<T>, string> errorMessageFactory,
                                   bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        if (check.IsShortCircuited || !check.IsValueNull && range.IsValueWithinRange(check.Value))
            return check;

        check = check.CreateAndAddError(errorMessageFactory, range);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is outside of the specified range, or otherwise adds an error message to the validation context.
    /// </summary>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">
    /// The value describing the allowed lower and upper boundaries.
    /// Use the static <see cref="Range" /> class to fluently create a <see cref="Range{T}" /> instance.
    /// </param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<T> IsNotIn<T>(this Check<T> check,
                                      Range<T> range,
                                      string? message = null,
                                      bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        if (check.IsShortCircuited || !check.IsValueNull && !range.IsValueWithinRange(check.Value))
            return check;

        check = check.AddNotInRangeError(range, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the value is outside of the specified range, or otherwise adds the error message that was created
    /// by the specified factory to the validation context.
    /// </summary>
    /// <typeparam name="T">The type of the value to be checked.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="range">
    /// The value describing the allowed lower and upper boundaries.
    /// Use the static <see cref="Range" /> class to fluently create a <see cref="Range{T}" /> instance.
    /// </param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> IsNotIn<T>(this Check<T> check,
                                      Range<T> range,
                                      Func<Check<T>, Range<T>, string> errorMessageFactory,
                                      bool shortCircuitOnError = false)
        where T : IComparable<T>
    {
        if (check.IsShortCircuited || !check.IsValueNull && !range.IsValueWithinRange(check.Value))
            return check;

        check = check.CreateAndAddError(errorMessageFactory, range);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }
}