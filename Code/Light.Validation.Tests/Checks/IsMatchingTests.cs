using System.Text.RegularExpressions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsMatchingTests
{
    public static readonly TheoryData<string?> InvalidValues =
        new ()
        {
            null,
            "",
            "123456",
            "ABC",
            "\tGrub"
        };

    public static readonly TheoryData<string> ValidValues =
        new ()
        {
            // ReSharper disable once StringLiteralTypo
            "abcd",
            "YMCA"
        };

    private static Regex Regex { get; } = new (@"^\w{4}$");

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidValue(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var check = context.Check(dto.Value).IsMatching(Regex);

        context.ShouldHaveSingleError("value", "value must match the required pattern.");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidValue(string validValue)
    {
        var (dto, context) = Test.SetupDefault(validValue);

        var check = context.Check(dto.Value).IsMatching(Regex);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var check = context.Check(dto.Value).IsMatching(Regex, "No match for you");

        context.ShouldHaveSingleError("value", "No match for you");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var check = context.Check(dto.Value).IsMatching(Regex, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorForShortCircuitedCheck(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsMatching(Regex);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessageFactory(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var check = context.Check(dto.Value).IsMatching(Regex, (c, r) => $"{c.Key} must match pattern \"{r}\".");

        context.ShouldHaveSingleError("value", "value must match pattern \"^\\w{4}$\".");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string validValue)
    {
        var (dto, context) = Test.SetupDefault(validValue);

        var check = context.Check(dto.Value).IsMatching(Regex, (_, _) => "GG");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var check = context.Check(dto.Value)
                           .IsMatching(Regex, (_, _) => "Here's the error", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsMatching(Regex, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}