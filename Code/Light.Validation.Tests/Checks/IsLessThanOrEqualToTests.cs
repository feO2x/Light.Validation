using System.Globalization;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsLessThanOrEqualToTests
{
    public static readonly TheoryData<float, float> InvalidValues =
        new ()
        {
            { 5f, 3f },
            { 19.509f, 19.508f },
            { 0f, -0.00001f }
        };

    public static readonly TheoryData<int, int> ValidValues =
        new ()
        {
            { 998, 999 },
            { -1000, -1000 },
            { 0, 1 },
            { 0, 0 }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueIsGreaterThan(float value, float comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLessThanOrEqualTo(comparativeValue);

        context.ShouldHaveSingleError("value", $"value must be less than or equal to {comparativeValue.ToString(CultureInfo.InvariantCulture)}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueIsLessThanOrEqualTo(int value, int comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLessThanOrEqualTo(comparativeValue);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(float value, float comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLessThanOrEqualTo(comparativeValue, "Errors.LessThanOrEqualTo");

        context.ShouldHaveSingleError("value", "Errors.LessThanOrEqualTo");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(float value, float comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLessThanOrEqualTo(comparativeValue, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(float value, float comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsLessThanOrEqualTo(comparativeValue);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var (dto, context) = Test.SetupDefault(24.42);

        var check = context.Check(dto.Value).IsLessThanOrEqualTo(24.1, (c, o) => $"{c.Key} is too great ({o.ToString(CultureInfo.InvariantCulture)})");

        context.ShouldHaveSingleError("value", "value is too great (24.1)");
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void NoErrorWithCustomFactory()
    {
        var (dto, context) = Test.SetupDefault('G');

        var check = context.Check(dto.Value).IsLessThanOrEqualTo('G', (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void ShortCircuitWithCustomFactory()
    {
        var (dto, context) = Test.SetupDefault('h');

        var check = context.Check(dto.Value)
                           .IsLessThanOrEqualTo('d', (_, _) => "Here's an error message", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void NoErrorOnShortCircuitedCheckWithCustomFactory()
    {
        var (dto, context) = Test.SetupDefault('Z');

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsLessThanOrEqualTo('X', (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}