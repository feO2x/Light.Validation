using System.Runtime.InteropServices.ComTypes;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class HasLengthInTests
{
    public static readonly TheoryData<string, Range<int>> InvalidValues =
        new ()
        {
            { "Hello World", Range.FromInclusive(1).ToInclusive(10) },
            { "This is too short", Range.FromExclusive(17).ToInclusive(25) },
            { "", Range.FromExclusive(0).ToInclusive(3) },
            { null!, Range.FromExclusive(0).ToInclusive(3) }
        };

    public static readonly TheoryData<string, Range<int>> ValidValues =
        new ()
        {
            { "Rule of Nines", Range.FromInclusive(5).ToInclusive(20) },
            { "Like a sword over Damocles", Range.FromExclusive(0).ToExclusive(50) }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void StringNotInRange(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).HasLengthIn(range);

        context.ShouldHaveSingleError("value", $"value must have {range.CreateRangeDescriptionText()} characters");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void StringInRange(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).HasLengthIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).HasLengthIn(range, "Custom Error");

        context.ShouldHaveSingleError("value", "Custom Error");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).HasLengthIn(range, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasLengthIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).HasLengthIn(range, (c, r) => $"{c.Key} must be between {r.From} and {r.To} characters long");

        context.ShouldHaveSingleError("value", $"value must be between {range.From} and {range.To} characters long");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).HasLengthIn(range, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).HasLengthIn(range, (_, _) => "My Custom Error", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(string value, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasLengthIn(range, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}