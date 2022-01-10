using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsEmailTests
{
    public static readonly TheoryData<string> InvalidValues =
        new ()
        {
            null!,
            "",
            "\t",
            "@only-domain.com",
            "without.domain@",
            "two@signs@",
            "sev@er@al@signs"
        };

    public static readonly TheoryData<string> ValidValues =
        new ()
        {
            "a@b",
            "kenny.pflug@live.de",
            "some-name@gmail.com",
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidEmailAddress(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        context.Check(dto.Value).IsEmail();

        context.ShouldHaveSingleError("value", "value must be an email address.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidEmailAddress(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        context.Check(dto.Value).IsEmail();

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        context.Check(dto.Value).IsEmail("Errors.InvalidEmail");

        context.ShouldHaveSingleError("value", "Errors.InvalidEmail");
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        context.Check(dto.Value).IsEmail(c => $"{c.Key} is no email address");

        context.ShouldHaveSingleError("value", "value is no email address");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        context.Check(dto.Value).IsEmail(_ => "Foo");

        context.ShouldHaveNoError();
    }
}