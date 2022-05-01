using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class ContainsOnlyLettersAndDigitsTests
{
    public static readonly TheoryData<string> InvalidValues =
        new ()
        {
            "ab10-29",
            "$1-bc",
            "100%",
            "",
            null!
        };

    public static readonly TheoryData<string> ValidValues =
        new ()
        {
            "1",
            "a",
            "20A3BF",
            "0xA83F"
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidString(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyLettersAndDigits();

        context.ShouldHaveSingleError("Value", "Value must contain only letters and digits");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidString(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyLettersAndDigits();

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyLettersAndDigits("My Custom Message");

        context.ShouldHaveSingleError("Value", "My Custom Message");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyLettersAndDigits(shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .ContainsOnlyLettersAndDigits();

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyLettersAndDigits(c => $"{c.Key} contains bad characters");

        context.ShouldHaveSingleError("Value", "Value contains bad characters");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyLettersAndDigits(_ => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyLettersAndDigits(_ => "It's wrong", true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .ContainsOnlyLettersAndDigits(_ => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}