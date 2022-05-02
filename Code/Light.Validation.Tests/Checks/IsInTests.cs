using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsInTests
{
    public static readonly TheoryData<decimal, Range<decimal>> InvalidValues =
        new ()
        {
            { 100m, Range.FromInclusive(50m).ToExclusive(100m) },
            { -90m, Range.FromInclusive(0m).ToInclusive(10m) },
            { 15.35m, Range.FromExclusive(15.35m).ToExclusive(25.75m) }
        };

    public static readonly TheoryData<int, Range<int>> ValidValues =
        new ()
        {
            { 42, Range.FromInclusive(40).ToExclusive(45) },
            { -502, Range.FromInclusive(-502).ToInclusive(0) },
            { 0, Range.FromInclusive(-1).ToInclusive(1) }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueNotInRange(decimal value, Range<decimal> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsIn(range);

        context.ShouldHaveSingleError("Value", $"Value must be in range from {range.CreateRangeDescriptionText()}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueInRange(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(decimal value, Range<decimal> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsIn(range, "My Custom Message");

        context.ShouldHaveSingleError("Value", "My Custom Message");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(decimal value, Range<decimal> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsIn(range, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(decimal value, Range<decimal> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(decimal value, Range<decimal> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsIn(range, (c, _) => $"{c.Key} must be in range!");

        context.ShouldHaveSingleError("Value", "Value must be in range!");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsIn(range, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(decimal value, Range<decimal> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsIn(range, (_, _) => "You're wrong!", shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(decimal value, Range<decimal> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsIn(range, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}