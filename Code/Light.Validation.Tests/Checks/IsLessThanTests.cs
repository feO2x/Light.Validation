using System;
using System.Globalization;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsLessThanTests
{
    public static readonly TheoryData<double, double> InvalidValues =
        new ()
        {
            { 5.1, 5.09 },
            { -1.776, -1.91 },
            { 0.00001, 0.0 },
            { 1.0, 1.0 }
        };

    public static readonly TheoryData<int, int> ValidValues =
        new ()
        {
            { 15, 16 },
            { -8, 4 },
            { 0, 1 },
            { int.MinValue, int.MaxValue }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueGreaterThanOrEqual(double value, double comparativeValue)
    {
        var dto = new Dto<double> { TheValue = value };
        var context = new ValidationContext();

        context.Check(dto.TheValue).IsLessThan(comparativeValue);

        context.ShouldHaveSingleError("theValue", $"theValue must be less than {comparativeValue.ToString(CultureInfo.InvariantCulture)}.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueIsLessThan(int value, int comparativeValue)
    {
        var dto = new Dto<int> { TheValue = value };
        var context = new ValidationContext();

        context.Check(dto.TheValue).IsLessThan(comparativeValue);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(double value, double comparativeValue)
    {
        var dto = new Dto<double> { TheValue = value };
        var context = new ValidationContext();

        context.Check(dto.TheValue).IsLessThan(comparativeValue, "Errors.LessThan");

        context.ShouldHaveSingleError("theValue", "Errors.LessThan");
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto<float> { TheValue = 42f };
        var context = new ValidationContext();

        context.Check(dto.TheValue).IsLessThan(40f, (c, o) => $"{c.Key} must not be greater than or equal to {o.ToString(CultureInfo.InvariantCulture)}");

        context.ShouldHaveSingleError("theValue", "theValue must not be greater than or equal to 40");
    }

    [Fact]
    public static void NoErrorWithCustomFactory()
    {
        var dto = new Dto<string> { TheValue = "a" };
        var context = new ValidationContext();

        context.Check(dto.TheValue).IsLessThan("b", (_, _) => "Foo");

        context.ShouldHaveNoError();
    }

    private sealed class Dto<T> where T : IComparable<T>
    {
        public T TheValue { get; init; } = default!;
    }
}