using System;
using System.Runtime.CompilerServices;
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

    /// <summary>
    /// Tries to parse the number to a value of the specified enum,
    /// or otherwise adds an error message to the validation context.
    /// If the number is parsed successfully, true will be returned,
    /// else false. This method will return false when the check is
    /// already short-circuited.
    /// </summary>
    /// <typeparam name="TEnum">The enum type the number should be parsed to.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="parsedEnumValue">The parsed enum value if parsing was successful.</param>
    /// <param name="message">
    /// The error message that will be added to the context (optional). If null is provided, the default error
    /// message will be created from the error templates associated to the validation context.
    /// </param>
    public static bool TryParseToEnum<TEnum>(this Check<int> check, out TEnum parsedEnumValue, string? message = null)
        where TEnum : struct, Enum
    {
        if (check.IsShortCircuited)
        {
            parsedEnumValue = default;
            return false;
        }

        parsedEnumValue = ConvertInt32ToEnum<TEnum>(check.Value);
        if (parsedEnumValue.IsValidEnumValue())
            return true;

        check.AddTryParseToEnumError(message);
        return false;
    }

    /// <summary>
    /// Tries to parse the number to a value of the specified enum,
    /// or otherwise adds an error message to the validation context.
    /// If the number is parsed successfully, true will be returned,
    /// else false. This method will return false when the check is
    /// already short-circuited.
    /// </summary>
    /// <typeparam name="TEnum">The enum type the number should be parsed to.</typeparam>
    /// <param name="check">The structure that encapsulates the value to be checked and the validation context.</param>
    /// <param name="parsedEnumValue">The parsed enum value if parsing was successful.</param>
    /// <param name="errorMessageFactory">The delegate that is used to create the error message.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessageFactory" /> is null.</exception>
    public static bool TryParseToEnum<TEnum>(this Check<int> check,
                                             out TEnum parsedEnumValue,
                                             Func<Check<int>, string> errorMessageFactory)
        where TEnum : struct, Enum
    {
        if (check.IsShortCircuited)
        {
            parsedEnumValue = default;
            return false;
        }

        parsedEnumValue = ConvertInt32ToEnum<TEnum>(check.Value);
        if (parsedEnumValue.IsValidEnumValue())
            return true;

        check.CreateAndAddError(errorMessageFactory);
        return false;
    }

    private static TEnum ConvertInt32ToEnum<TEnum>(int value)
        where TEnum : struct, Enum
    {
        var enumSize = Unsafe.SizeOf<TEnum>();
        switch (enumSize)
        {
            case 1:
                var byteValue = (byte)value;
                return Unsafe.As<byte, TEnum>(ref byteValue);
            case 2:
                var shortValue = (short)value;
                return Unsafe.As<short, TEnum>(ref shortValue);
            case 4:
                return Unsafe.As<int, TEnum>(ref value);
            case 8:
                long longValue = value;
                return Unsafe.As<long, TEnum>(ref longValue);
            default:
                throw new InvalidOperationException($"The enum type \"{typeof(TEnum)}\" has an unknown size of {enumSize}.");
        }
    }
}