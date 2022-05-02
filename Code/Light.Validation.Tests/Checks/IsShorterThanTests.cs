using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsShorterThanTests
{
    public static readonly TheoryData<string, int> InvalidValues =
        new ()
        {
            { "Hello World", 3 },
            { "So short", 5 },
            { "0", 1 },
            { "", 0 },
            { null!, 0 }
        };

    public static readonly TheoryData<string, int> ValidValues =
        new ()
        {
            { "One", 4 },
            { "Long, but not long enough", 30 },
            { "", 1 }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void StringNotShortEnough(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsShorterThan(length);

        context.ShouldHaveSingleError("Value", $"Value must be shorter than {length} characters");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void StringShortEnough(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsShorterThan(length);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsShorterThan(length, "Custom Error Message");

        context.ShouldHaveSingleError("Value", "Custom Error Message");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsShorterThan(length, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsShorterThan(length);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsShorterThan(length, (c, l) => $"{c.Key} is too long ({l})");

        context.ShouldHaveSingleError("Value", $"Value is too long ({length})");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsShorterThan(length, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsShorterThan(length, (_, _) => "My Custom Error", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsShorterThan(length, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}