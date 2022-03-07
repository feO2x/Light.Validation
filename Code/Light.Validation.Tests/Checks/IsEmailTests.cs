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

        var check = context.Check(dto.Value).IsEmail();

        context.ShouldHaveSingleError("value", "value must be an email address.");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidEmailAddress(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value).IsEmail();

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value).IsEmail("Errors.InvalidEmail");

        context.ShouldHaveSingleError("value", "Errors.InvalidEmail");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value).IsEmail(shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsEmail();

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value).IsEmail(c => $"{c.Key} is no email address");

        context.ShouldHaveSingleError("value", "value is no email address");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value).IsEmail(_ => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value).IsEmail(_ => "Bar", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(string email)
    {
        var (dto, context) = Test.SetupDefault(email);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsEmail(_ => "Baz");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}