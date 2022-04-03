using System;
using Light.GuardClauses;
using Light.Validation.Tools;

namespace Light.Validation.Checks;

public static partial class Checks
{
    /// <summary>
    /// Rounds the double-precision floating-point value to a specified number of fractional
    /// digits using the specified rounding conventions.
    /// </summary>
    /// <param name="check">The check that encapsulates the double-precision value.</param>
    /// <param name="digits">The number of fractional digits in the return value. The default value is 2.</param>
    /// <param name="mode">
    /// One of the enumeration values that specifies which rounding strategy to use. The default value
    /// is <see cref="MidpointRounding.AwayFromZero" /> which rounds e.g. 1.735 to 1.74 (with two fractional digits).
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="digits" /> is less than 0 or greater than 15.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="mode" /> is not a valid value of <see cref="MidpointRounding" />.</exception>
    public static Check<double> Round(this Check<double> check,
                                      int digits = 2,
                                      MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        if (check.IsShortCircuited)
            return check;

        var newValue = Math.Round(check.Value, digits, mode);
        return check.WithNewValue(newValue);
    }

    /// <summary>
    /// Rounds the single-precision floating-point value to a specified number of fractional
    /// digits using the specified rounding conventions.
    /// </summary>
    /// <param name="check">The check that encapsulates the single-precision value.</param>
    /// <param name="digits">The number of fractional digits in the return value. The default value is 2.</param>
    /// <param name="mode">
    /// One of the enumeration values that specifies which rounding strategy to use. The default value
    /// is <see cref="MidpointRounding.AwayFromZero" /> which rounds e.g. 1.735 to 1.74 (with two fractional digits).
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="digits" /> is less than 0 or greater than 6.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="mode" /> is not a valid value of <see cref="MidpointRounding" />.</exception>
    public static Check<float> Round(this Check<float> check,
                                     int digits = 2,
                                     MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        if (check.IsShortCircuited)
            return check;

#if NETSTANDARD2_0
        digits.MustBeIn(Range.FromInclusive(0).ToInclusive(6));
        var newValue = (float) Math.Round(check.Value, digits, mode);
#else
        var newValue = MathF.Round(check.Value, digits, mode);
#endif
        return check.WithNewValue(newValue);
    }

    /// <summary>
    /// Rounds a decimal value to a specified number of fractional digits. A parameter
    /// specifies how to round the value if it is midway between two numbers.
    /// </summary>
    /// <param name="check">The check that encapsulates the decimal value.</param>
    /// <param name="decimals">The number of decimal places in the return value.</param>
    /// <param name="mode">
    /// One of the enumeration values that specifies which rounding strategy to use. The default value
    /// is <see cref="MidpointRounding.AwayFromZero" /> which rounds e.g. 1.735 to 1.74 (with two fractional digits).
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="decimals" /> is less than 0 or greater than 6.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="mode" /> is not a valid value of <see cref="MidpointRounding" />.</exception>
    public static Check<decimal> Round(this Check<decimal> check,
                                       int decimals,
                                       MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        if (check.IsShortCircuited)
            return check;

        var newValue = Math.Round(check.Value, decimals, mode);
        return check.WithNewValue(newValue);
    }
}