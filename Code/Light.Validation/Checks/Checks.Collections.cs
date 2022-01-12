using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Light.Validation.Tools;

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
    /// <typeparam name="T">The collection type of the value to be checked.</typeparam>
    public static Check<T> HasCount<T>(this Check<T> check, int count, string? message = null)
        where T : IEnumerable
    {
        if (check.IsValueNull)
            return check;

        var actualCount = check.Value.GetCount();
        if (actualCount != count)
            check.AddCountError(count, message);
        return check;
    }

    /// <summary>
    /// Checks if the collection has the specified count, or otherwise adds an error message
    /// that was created by the specified factory to the validation context.
    /// </summary>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="count">The comparative value the actual collection count is compared to.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <typeparam name="T">The collection type of the value to be checked.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static Check<T> HasCount<T>(this Check<T> check,
                                       int count,
                                       Func<Check<T>, int, string> errorMessageFactory)
        where T : IEnumerable
    {
        if (check.IsValueNull)
            return check;

        var actualCount = check.Value.GetCount();
        if (actualCount != count)
            check.AddError(errorMessageFactory, count);
        return check;
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