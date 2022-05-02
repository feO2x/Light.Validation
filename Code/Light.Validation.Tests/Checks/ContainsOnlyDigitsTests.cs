using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class ContainsOnlyDigitsTests
{
    public static readonly TheoryData<string> InvalidValues =
        new ()
        {
            "abc",
            "119-a",
            "B19H",
            "",
            null!
        };

    public static readonly TheoryData<string> ValidValues =
        new ()
        {
            "1",
            "2009",
            "0",
            "1954829193485762982389592392191487582"
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidString(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyDigits();

        context.ShouldHaveSingleError("Value", "Value must contain only digits");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidStrings(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyDigits();

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyDigits("My Custom Message");

        context.ShouldHaveSingleError("Value", "My Custom Message");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyDigits(shortCircuitOnError: true);

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
                           .ContainsOnlyDigits();

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyDigits(c => $"{c.Key} contains not only digits, my friend");

        context.ShouldHaveSingleError("Value", "Value contains not only digits, my friend");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyDigits(_ => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(string value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).ContainsOnlyDigits(_ => "It's wrong", true);

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
                           .ContainsOnlyDigits(_ => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}