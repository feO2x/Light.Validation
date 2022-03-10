using System;
using Light.GuardClauses;

namespace Light.Validation.Tools;

/// <summary>
/// Defines a range that can be used to check if a specified <see cref = "IComparable{T}"/> is in between it or not.
/// </summary>
/// <typeparam name="T">The type that the range should be applied to.</typeparam>
public readonly record struct Range<T> where T : IComparable<T>
{
    /// <summary>
    /// Defines a range that can be used to check if a specified <see cref = "IComparable{T}"/> is in between it or not.
    /// </summary>
    /// <param name="from">The lower boundary of the range.</param>
    /// <param name="to">The upper boundary of the range.</param>
    /// <param name="isFromInclusive">The value indicating whether <paramref name = "from"/> is inclusive or exclusive.</param>
    /// <param name="isToInclusive">The value indicating whether <paramref name = "to"/> is inclusive or exclusive.</param>
    public Range(T from,
                 T to,
                 bool isFromInclusive,
                 bool isToInclusive)
    {
        From = from.MustNotBeNullReference();
        To = to.MustNotBeLessThan(from);
        IsFromInclusive = isFromInclusive;
        IsToInclusive = isToInclusive;
    }

    /// <summary>
    /// The lower boundary of the range.
    /// </summary>
    public T From { get; }

    /// <summary>
    /// The upper boundary of the range.
    /// </summary>
    public T To { get; }

    /// <summary>
    /// Gets the value indicating whether <see cref="From"/> is inclusive or exclusive.
    /// </summary>
    public bool IsFromInclusive { get; }

    /// <summary>
    /// The value indicating whether <see cref="To"/> is inclusive or exclusive.
    /// </summary>
    public bool IsToInclusive { get; }

    /// <summary>
    /// Checks if the specified <paramref name = "value"/> is within range.
    /// </summary>
    /// <param name = "value">The value to be checked.</param>
    public bool IsValueWithinRange(T value) =>
        value.MustNotBeNullReference().CompareTo(From) >= GetLowerBoundaryThreshold() &&
        value.CompareTo(To) <= GetUpperBoundaryThreshold();

    private int GetLowerBoundaryThreshold() => IsFromInclusive ? 0 : 1;
    private int GetUpperBoundaryThreshold() => IsToInclusive ? 0 : -1;

    /// <inheritdoc />
    public override string ToString() => "Range from " + CreateRangeDescriptionText();

    /// <summary>
    /// Returns a text description of this range with the following pattern: From (inclusive | exclusive) to To (inclusive | exclusive).
    /// </summary>
    public string CreateRangeDescriptionText(string fromToConnectionWord = "to") =>
        From + " (" + GetLowerBoundaryType() + ") " + fromToConnectionWord + " " + To + " (" + GetUpperBoundaryType() + ")";

    private string GetLowerBoundaryType() => GetBoundaryText(IsFromInclusive);
    private string GetUpperBoundaryType() => GetBoundaryText(IsToInclusive);
    private static string GetBoundaryText(bool isInclusive) => isInclusive ? "inclusive" : "exclusive";

    /// <summary>
    /// The nested <see cref="RangeFromInfo" /> can be used to fluently create a <see cref="Range{T}" />.
    /// </summary>
    /// <param name="From">The lower boundary of the range.</param>
    /// <param name="IsFromInclusive">The value indicating whether <paramref name = "From"/> is inclusive or exclusive.</param>
    public readonly record struct RangeFromInfo(T From, bool IsFromInclusive)
    {
        /// <summary>
        /// Creates a range with the specified upper boundary as an inclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the inclusive upper boundary of the resulting range.</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "value"/> is less than the lower boundary value.
        /// </exception>
        public Range<T> ToInclusive(T value) => new (From, value, IsFromInclusive, true);

        /// <summary>
        /// Creates a range with the specified upper boundary as an exclusive value.
        /// </summary>
        /// <param name="value">The value that indicates the exclusive upper boundary of the resulting range.</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "value"/> is less than the lower boundary value.
        /// </exception>
        public Range<T> ToExclusive(T value) => new (From, value, IsFromInclusive, false);
    }
}

/// <summary>
/// Provides methods to simplify the creation of <see cref = "Range{T}" /> instances.
/// </summary>
public static class Range
{
    /// <summary>
    /// Use this method to create a range in a fluent style using method chaining.
    /// Defines the lower boundary as an inclusive value.
    /// </summary>
    /// <param name="value">The value that indicates the inclusive lower boundary of the resulting range.</param>
    /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
    public static Range<T>.RangeFromInfo FromInclusive<T>(T value)
        where T : IComparable<T> => new (value, true);

    /// <summary>
    /// Use this method to create a range in a fluent style using method chaining.
    /// Defines the lower boundary as an exclusive value.
    /// </summary>
    /// <param name = "value">The value that indicates the exclusive lower boundary of the resulting range.</param>
    /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
    public static Range<T>.RangeFromInfo FromExclusive<T>(T value)
        where T : IComparable<T> => new (value, false);
}