using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsNullOrWhiteSpaceTests
{
    public static readonly TheoryData<string> InvalidValues =
        new ()
        {
            null!,
            string.Empty,
            "\t",
            " \r\n"
        };

    public static readonly TheoryData<string> ValidValues =
        new ()
        {
            "Foo",
            " Bar ",
            "Baz\n",
            "kenny.pflug@live.de"
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValueIsNullEmptyOrWhiteSpace(string input)
    {
        var (dto, context) = Test.SetupDefault(input);

        context.Check(dto.Value).IsNotNullOrWhiteSpace();

        context.ShouldHaveSingleError("value", "value must not be empty.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValueIsNotEmpty(string input)
    {
        var (dto, context) = Test.SetupDefault(input);

        context.Check(dto.Value).IsNotNullOrWhiteSpace();

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(string input)
    {
        var (dto, context) = Test.SetupDefault(input);

        context.Check(dto.Value).IsNotNullOrWhiteSpace("Errors.EmptyString");

        context.ShouldHaveSingleError("value", "Errors.EmptyString");
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string input)
    {
        var (dto, context) = Test.SetupDefault(input);

        context.Check(dto.Value).IsNotNullOrWhiteSpace(c => $"You must set the {c.Key}");

        context.ShouldHaveSingleError("value", "You must set the value");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string input)
    {
        var (dto, context) = Test.SetupDefault(input);

        context.Check(dto.Value).IsNotNullOrWhiteSpace(_ => "Foo");

        context.ShouldHaveNoError();
    }
}