using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsLongerThanTests
{
    public static readonly TheoryData<string, int> InvalidValues =
        new ()
        {
            { "Hello", 5 },
            { "This is pretty long", 25 },
            { "But not long enough", 30 },
            { "", 0 }
        };

    public static readonly TheoryData<string, int> ValidValues =
        new ()
        {
            { "Oh, shut up", 10 },
            // ReSharper disable once StringLiteralTypo
            { "Ideological? Why don't we call it putinilogical?", 20 }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void StringNotLongEnough(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLongerThan(length);

        context.ShouldHaveSingleError("value", $"value must be longer than {length}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void StringLongEnough(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLongerThan(length);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLongerThan(length, "Custom Error Message");

        context.ShouldHaveSingleError("value", "Custom Error Message");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLongerThan(length, shortCircuitOnError: true);

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
                           .IsLongerThan(length);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLongerThan(length, (c, l) => $"{c.Key} is too short ({l})");

        context.ShouldHaveSingleError("value", $"value is too short ({length})");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLongerThan(length, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(string value, int length)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsLongerThan(length, (_, _) => "My Custom Error", true);

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
                           .IsLongerThan(length, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}