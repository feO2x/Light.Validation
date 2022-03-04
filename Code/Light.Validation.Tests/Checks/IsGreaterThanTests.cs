using System;
using System.Globalization;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsGreaterThanTests
{
    public static readonly TheoryData<int, int> InvalidValues =
        new ()
        {
            { 42, 42 },
            { 16, 18 },
            { 0, 1 },
            { -1, 0 },
            { int.MinValue, 0 }
        };

    public static readonly TheoryData<decimal, decimal> ValidValues =
        new ()
        {
            { 2m, 1m },
            { decimal.MaxValue, decimal.MinValue },
            { 18.000_001m, 18.000_000m },
            { -12m, -25_398m }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueIsLessThanOrEqualTo(int value, int comparativeValue)
    {
        var dto = new Dto<int> { SomeValue = value };
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsGreaterThan(comparativeValue);

        context.ShouldHaveSingleError("someValue", $"someValue must be greater than {comparativeValue}.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueIsGreaterThan(decimal value, decimal comparativeValue)
    {
        var dto = new Dto<decimal> { SomeValue = value };
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsGreaterThan(comparativeValue);

        context.ShouldHaveNoErrors();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(int value, int comparativeValue)
    {
        var dto = new Dto<int> { SomeValue = value };
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsGreaterThan(comparativeValue, "It must be greater!");

        context.ShouldHaveSingleError("someValue", "It must be greater!");
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto<double> { SomeValue = 1.337 };
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsGreaterThan(2.701, (c, o) => $"{c.Key} must not be less than or equal to {o.ToString(CultureInfo.InvariantCulture)}, but you provided {c.Value.ToString(CultureInfo.InvariantCulture)}");

        context.ShouldHaveSingleError("someValue", "someValue must not be less than or equal to 2.701, but you provided 1.337");
    }

    [Fact]
    public static void NoErrorWithCustomMessageFactory()
    {
        var dto = new Dto<string> { SomeValue = "Z" };
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsGreaterThan("z", (_, _) => "I don't care");

        context.ShouldHaveNoErrors();
    }
    
    private sealed class Dto<T> where T : IComparable<T>
    {
        public T SomeValue { get; init; } = default!;
    }
}