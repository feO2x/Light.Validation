using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsNotInTests
{
    public static readonly TheoryData<int, Range<int>> InvalidValues =
        new ()
        {
            { 5, Range.FromInclusive(0).ToInclusive(10) },
            { 0, Range.FromExclusive(-1).ToInclusive(2) },
            { 1520, Range.FromInclusive(1000).ToInclusive(1530) }
        };

    public static readonly TheoryData<long, Range<long>> ValidValues =
        new ()
        {
            { 0L, Range.FromInclusive(1L).ToInclusive(10L) },
            { -20L, Range.FromExclusive(-20L).ToInclusive(-5L) },
            { long.MaxValue, Range.FromInclusive(0L).ToExclusive(long.MaxValue) }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueInRange(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotIn(range);

        context.ShouldHaveSingleError("Value", $"Value must not be in range from {range.CreateRangeDescriptionText()}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueNotInRange(long value, Range<long> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotIn(range, "Custom Error Message");

        context.ShouldHaveSingleError("Value", "Custom Error Message");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotIn(range, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsNotIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .IsNotIn(range, (c, r) => $"{c.Key} should not be in range from {r.From} to {r.To}");

        context.ShouldHaveSingleError("Value", $"Value should not be in range from {range.From} to {range.To}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(long value, Range<long> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .IsNotIn(range, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .IsNotIn(range, (_, _) => "My custom error", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(int value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsNotIn(range, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}