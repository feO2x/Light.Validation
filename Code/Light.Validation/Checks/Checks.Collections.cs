using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Light.GuardClauses;
using Light.Validation.Tools;
using Index = Light.Validation.Tools.Index;

namespace Light.Validation.Checks;

public static partial class Checks
{
    /// <summary>
    /// Checks if the collection has the specified count, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="count">The comparative value the actual collection count is compared to.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The collection type of the value to be checked.</typeparam>
    public static Check<T> HasCount<T>(this Check<T> check,
                                       int count,
                                       string? message = null,
                                       bool shortCircuitOnError = false)
        where T : IEnumerable
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.GetCount() == count)
            return check;

        check = check.AddCountError(count, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the collection has the specified count, or otherwise adds an error message
    /// that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="count">The comparative value the actual collection count is compared to.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <typeparam name="T">The collection type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> HasCount<T>(this Check<T> check,
                                       int count,
                                       Func<Check<T>, int, string> errorMessageFactory,
                                       bool shortCircuitOnError = false)
        where T : IEnumerable
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.GetCount() == count)
            return check;
        
        check = check.CreateAndAddError(errorMessageFactory, count);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the collection has at least the specified count, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="minimumCount">The minimum count the collection should have.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<T> HasMinimumCount<T>(this Check<T> check,
                                              int minimumCount,
                                              string? message = null,
                                              bool shortCircuitOnError = false)
        where T : IEnumerable
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.GetCount() >= minimumCount)
            return check;
        
        check = check.AddMinimumCountError(minimumCount, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the collection has at least the specified count, or otherwise adds an error message
    /// that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="minimumCount">The minimum count the collection should have.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> HasMinimumCount<T>(this Check<T> check,
                                              int minimumCount,
                                              Func<Check<T>, int, string> errorMessageFactory,
                                              bool shortCircuitOnError = false)
        where T : IEnumerable
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.GetCount() >= minimumCount)
            return check;

        check = check.CreateAndAddError(errorMessageFactory, minimumCount);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the collection has at most the specified count, or otherwise adds an error message
    /// to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="maximumCount">The maximum count the collection should have.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    public static Check<T> HasMaximumCount<T>(this Check<T> check,
                                              int maximumCount,
                                              string? message = null,
                                              bool shortCircuitOnError = false)
        where T : IEnumerable
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.GetCount() <= maximumCount)
            return check;

        check = check.AddMaximumCountError(maximumCount, message);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Checks if the collection has at most the specified count, or otherwise adds an error message
    /// that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="maximumCount">The maximum count the collection should have.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> HasMaximumCount<T>(this Check<T> check,
                                              int maximumCount,
                                              Func<Check<T>, int, string> errorMessageFactory,
                                              bool shortCircuitOnError = false)
        where T : IEnumerable
    {
        if (check.IsShortCircuited || !check.IsValueNull && check.Value.GetCount() <= maximumCount)
            return check;

        check = check.CreateAndAddError(errorMessageFactory, maximumCount);
        return check.ShortCircuitIfNecessary(shortCircuitOnError);
    }

    /// <summary>
    /// Validates each item of the collection with the specified <paramref name="validate" /> delegate.
    /// Before the collection is iterated, a null check is performed unless you set <paramref name="isNullCheckingEnabled" />
    /// to false.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection. The collection must implement <see cref="IList{T}" />.</typeparam>
    /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="validate">The delegate that validates each item in the collection.</param>
    /// <param name="isNullCheckingEnabled">
    /// The value indicating whether an automatic null check on the collection should be performed (optional).
    /// The default value is true.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="validate" /> is null.</exception>
    public static Check<TCollection> ValidateItems<TCollection, TValue>(this Check<TCollection> check,
                                                                        Func<Check<TValue>, Check<TValue>> validate,
                                                                        bool isNullCheckingEnabled = true,
                                                                        bool shortCircuitOnError = false)
        where TCollection : IList<TValue>
    {
        validate.MustNotBeNull();

        if (check.IsShortCircuited)
            return check;


        if (isNullCheckingEnabled && check.IsValueNull)
        {
            check = check.NormalizeKeyIfNecessary();
            var error = check.Context.CreateErrorForAutomaticNullCheck(check.Key, check.DisplayName);
            check = check.AddError(error);
            return check.ShortCircuitIfNecessary(shortCircuitOnError);
        }

        var childContext = check.CreateChildContext();
        var list = check.Value;
        for (var i = 0; i < list.Count; i++)
        {
            var item = list[i];
            var itemCheck = childContext.Check(item, key: Index.ToStringFast(i), displayName: "The value");
            itemCheck = validate(itemCheck);
            list[i] = itemCheck.Value;
        }

        return childContext.TryGetErrors(out var errors) ?
                   check.NormalizeKeyIfNecessary()
                        .AddError(errors)
                        .ShortCircuitIfNecessary(shortCircuitOnError) :
                   check;
    }

    /// <summary>
    /// Validates each item of the collection asynchronously with the specified <paramref name="validateAsync" /> delegate.
    /// Before the collection is iterated, a null check is performed unless you set <paramref name="isNullCheckingEnabled" />
    /// to false.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection. The collection must implement <see cref="IList{T}" />.</typeparam>
    /// <typeparam name="TValue">The type of the items in the collection.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="validateAsync">The delegate that validates each item in the collection.</param>
    /// <param name="isNullCheckingEnabled">
    /// The value indicating whether an automatic null check on the collection should be performed (optional).
    /// The default value is true.
    /// </param>
    /// <param name="shortCircuitOnError">
    /// The value indicating whether the check instance is short-circuited when validation fails.
    /// Short-circuited instances will not perform any more checks.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="validateAsync" /> is null.</exception>
    public static async
#if NETSTANDARD2_0
        Task<Check<TCollection>>
#else
        ValueTask<Check<TCollection>>
#endif
        ValidateItemsAsync<TCollection, TValue>(this Check<TCollection> check,
                                                Func<Check<TValue>, Task<Check<TValue>>> validateAsync,
                                                bool isNullCheckingEnabled = true,
                                                bool shortCircuitOnError = false)
        where TCollection : IList<TValue>
    {
        validateAsync.MustNotBeNull();

        if (check.IsShortCircuited)
            return check;

        if (isNullCheckingEnabled && check.IsValueNull)
        {
            check = check.NormalizeKeyIfNecessary();
            var error = check.Context.CreateErrorForAutomaticNullCheck(check.Key, check.DisplayName);
            check = check.AddError(error);
            return check.ShortCircuitIfNecessary(shortCircuitOnError);
        }

        var childContext = check.CreateChildContext();
        var list = check.Value;
        for (var i = 0; i < list.Count; i++)
        {
            var item = list[i];
            var itemCheck = childContext.Check(item, key: Index.ToStringFast(i), displayName: "The value");
            itemCheck = await validateAsync(itemCheck);
            list[i] = itemCheck.Value;
        }

        return childContext.TryGetErrors(out var errors) ?
                   check.NormalizeKeyIfNecessary()
                        .AddError(errors)
                        .ShortCircuitIfNecessary(shortCircuitOnError) :
                   check;
    }

    private static int GetCount<T>(this T enumerable)
        where T : IEnumerable
    {
        if (typeof(T) == typeof(string))
            return Unsafe.As<T, string>(ref enumerable).Length;

        if (enumerable is ICollection collection)
            return collection.Count;

        return CountByIterating(enumerable);

        static int CountByIterating(T enumerable)
        {
            var count = 0;
            var enumerator = enumerable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                    count++;
            }
            finally
            {
                if (enumerable is IDisposable disposable)
                    disposable.Dispose();
            }

            return count;
        }
    }
}