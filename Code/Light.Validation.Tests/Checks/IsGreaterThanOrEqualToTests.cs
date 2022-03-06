using System.Globalization;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsGreaterThanOrEqualToTests
{
    public static readonly TheoryData<int, int> InvalidValues =
        new ()
        {
            { 30, 45 },
            { -12, -11 },
            { 0, 1 },
            { -1, 0 }
        };

    public static readonly TheoryData<decimal, decimal> ValidValues =
        new ()
        {
            { 2.075m, 2.075m },
            { 15m, 10.5m },
            { 1.5722m, -0.319m }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueIsLess(int value, int comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsGreaterThanOrEqualTo(comparativeValue);

        context.ShouldHaveSingleError(
            "value", $"value must be greater than or equal to {comparativeValue.ToString()}.");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueIsGreaterOrEqual(decimal value, decimal comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsGreaterThanOrEqualTo(comparativeValue);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(int value, int comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsGreaterThanOrEqualTo(comparativeValue, "Errors.MustBeGreaterThanOrEqualTo");

        context.ShouldHaveSingleError("value", "Errors.MustBeGreaterThanOrEqualTo");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(int value, int comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsGreaterThanOrEqualTo(comparativeValue);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(int value, int comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsGreaterThanOrEqualTo(comparativeValue, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var (dto, context) = Test.SetupDefault(42.055);

        var check = context
                   .Check(dto.Value)
                   .IsGreaterThanOrEqualTo(
                        45.75,
                        (c, o) => $"{c.Key} must not be less than {o.ToString(CultureInfo.InvariantCulture)}"
                    );

        context.ShouldHaveSingleError("value", "value must not be less than 45.75");
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void NoErrorWithCustomMessageFactory()
    {
        var (dto, context) = Test.SetupDefault(-15f);

        var check = context.Check(dto.Value).IsGreaterThanOrEqualTo(-19.5f, (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void ShortCircuitWithCustomMessageFactory()
    {
        var (dto, context) = Test.SetupDefault("a");

        var check = context.Check(dto.Value)
                           .IsGreaterThanOrEqualTo("c", (_, _) => "The error", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory()
    {
        var (dto, context) = Test.SetupDefault("Y");

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsGreaterThanOrEqualTo("Z", (_, _) => "whatever", true);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}