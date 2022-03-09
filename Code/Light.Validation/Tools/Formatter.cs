using System.Globalization;
using System.Runtime.CompilerServices;

namespace Light.Validation.Tools;

/// <summary>
/// Provides an extension method to format generic values.
/// </summary>
public static class Formatter
{
    /// <summary>
    /// Calls ToString on the value and returns it. If the value is either double, float, or decimal,
    /// ToString will be called with the specified culture info.
    /// </summary>
    public static string Format<T>(T value, CultureInfo cultureInfo)
    {
        if (typeof(T) == typeof(double))
            return Unsafe.As<T, double>(ref value).ToString(cultureInfo);
        if (typeof(T) == typeof(float))
            return Unsafe.As<T, float>(ref value).ToString(cultureInfo);
        if (typeof(T) == typeof(decimal))
            return Unsafe.As<T, decimal>(ref value).ToString(cultureInfo);

        return value is null ? "null" : value.ToString();
    }

    /// <summary>
    /// Calls ToString on the value and returns it. If the value is either double, float, or decimal,
    /// ToString will be called with the invariant culture info.
    /// </summary>
    public static string Format<T>(T value) => Format(value, CultureInfo.InvariantCulture);
}