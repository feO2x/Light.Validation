using System;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsEqualToTests
{
    public static readonly TheoryData<string, string> OrdinalInvalidValues =
        new ()
        {
            { "foo", "Foo" },
            { "BAR", "bar" },
            { "Baz", "Qux" },
            { "", null! },
            { null!, "\t" }
        };

    public static readonly TheoryData<int> ValidValues =
        new ()
        {
            42,
            int.MinValue,
            int.MaxValue,
            0,
            1
        };

    public static readonly TheoryData<string, string> IgnoreCaseInvalidValues =
        new ()
        {
            { "one", "two" },
            { "", "\n" },
            { null!, "" },
            { "", null! },
            { "A very long input", "A very long comparison" }
        };

    public static readonly TheoryData<string, string> IgnoreCaseValidValues =
        new ()
        {
            { "Similar", "Similar" },
            { "Equal", "EQUAL" },
            { "", "" },
            { null!, null! }
        };

    [Theory]
    [MemberData(nameof(OrdinalInvalidValues))]
    public static void ValuesNotEqualWithDefaultEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(comparativeValue);

        context.ShouldHaveSingleError("value", $"value must be {Formatter.Format(comparativeValue)}.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValuesEqual(int value)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(value);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(OrdinalInvalidValues))]
    public static void CustomErrorMessage(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(comparativeValue, "Errors.ValuesNotEqual");

        context.ShouldHaveSingleError("value", "Errors.ValuesNotEqual");
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void ValuesNotEqualWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase);

        context.ShouldHaveSingleError("value", $"value must be {Formatter.Format(comparativeValue)}.");
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseValidValues))]
    public static void ValuesEqualWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void CustomErrorMessageWithCustomComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase, "Errors.ValuesNotEqual");

        context.ShouldHaveSingleError("value", "Errors.ValuesNotEqual");
    }

    [Theory]
    [MemberData(nameof(OrdinalInvalidValues))]
    public static void CustomMessageFactory(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(comparativeValue, (c, o) => $"{c.Key} is not {Formatter.Format(o)}");

        context.ShouldHaveSingleError("value", $"value is not {Formatter.Format(comparativeValue)}");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(int value)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsEqualTo(value, (_, _) => "Foo");

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void CustomMessageFactoryWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value)
               .IsEqualTo(comparativeValue,
                          StringComparer.OrdinalIgnoreCase,
                          (c, o) => $"{c.Key} is not {Formatter.Format(o)}");

        context.ShouldHaveSingleError("value", $"value is not {Formatter.Format(comparativeValue)}");
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseValidValues))]
    public static void NoErrorWithCustomFactoryAndCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value)
               .IsEqualTo(comparativeValue,
                          StringComparer.OrdinalIgnoreCase,
                          (_, _) => "Foo");

        context.ShouldHaveNoError();
    }
}