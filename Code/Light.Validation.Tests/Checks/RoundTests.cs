using FluentAssertions;
using Light.Validation.Checks;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class RoundTests
{
    [Theory]
    [InlineData(3.579, 2, 3.58)]
    [InlineData(593.9364, 3, 593.936)]
    [InlineData(-12.399, 1, -12.4)]
    [InlineData(241.9965, 0, 242.0)]
    [InlineData(3.1, 2, 3.1)]
    public static void RoundDouble(double value, int digits, double expectedValue)
    {
        var context = ValidationContextFactory.CreateContext();

        var roundedValue = context.Check(value).Round(digits).Value;

        roundedValue.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(4.22f, 1, 4.2f)]
    [InlineData(235.63984f, 3, 235.640f)]
    [InlineData(-14.56, 0, -15.0f)]
    [InlineData(42.25f, 3, 42.25f)]
    public static void RoundFloat(float value, int digits, float expectedValue)
    {
        var context = ValidationContextFactory.CreateContext();

        var roundedValue = context.Check(value).Round(digits).Value;

        roundedValue.Should().Be(expectedValue);
    }

    [Theory]
    [MemberData(nameof(DecimalData))]
    public static void RoundDecimal(decimal value, int digits, decimal expectedValue)
    {
        var context = ValidationContextFactory.CreateContext();

        var roundedValue = context.Check(value).Round(digits).Value;

        roundedValue.Should().Be(expectedValue);
    }

    public static readonly TheoryData<decimal, int, decimal> DecimalData =
        new ()
        {
            { 45.45692m, 2, 45.46m },
            { 1592.234m, 1, 1592.2m },
            { -19.44m, 1, -19.4m },
            { 5.00000909m, 5, 5.00001m },
            { 15.24m, 4, 15.24m }
        };
}