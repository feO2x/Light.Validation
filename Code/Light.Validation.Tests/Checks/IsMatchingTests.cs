using System.Text.RegularExpressions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsMatchingTests
{
    public static readonly TheoryData<string> InvalidValues =
        new ()
        {
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

        context.Check(dto.Value).IsMatching(Regex);

        context.ShouldHaveSingleError("value", "value must match the required pattern.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidValue(string validValue)
    {
        var (dto, context) = Test.SetupDefault(validValue);

        context.Check(dto.Value).IsMatching(Regex);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        context.Check(dto.Value).IsMatching(Regex, "No match for you");

        context.ShouldHaveSingleError("value", "No match for you");
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessageFactory(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        context.Check(dto.Value).IsMatching(Regex, (c, r) => $"{c.Key} must match pattern \"{r}\".");

        context.ShouldHaveSingleError("value", "value must match pattern \"^\\w{4}$\".");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string validValue)
    {
        var (dto, context) = Test.SetupDefault(validValue);

        context.Check(dto.Value).IsMatching(Regex, (_, _) => "GG");

        context.ShouldHaveNoError();
    }
}