using System;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsNotEqualToTests
{
    public static readonly TheoryData<short> InvalidValues =
        new ()
        {
            0,
            24,
            -155,
            short.MinValue,
            short.MaxValue
        };

    public static readonly TheoryData<string, string> ValidValues =
        new ()
        {
            { "Foo", "Bar" },
            { "BAZ", "qux" },
            { "", "\t" },
            { "", null! },
            { null!, "" }
        };

    public static readonly TheoryData<string, string> IgnoreCaseInvalidValues =
        new ()
        {
            { "Foo", "foo" },
            { "bar", "BAR" },
            { "", "" }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ValuesEqualWithDefaultEqualityComparer(short value)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(value);

        context.ShouldHaveSingleError("value", $"value must not be {Formatter.Format(value)}.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValuesNotEqualWithDefaultEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(comparativeValue);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(short value)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(value, "WTF");

        context.ShouldHaveSingleError("value", "WTF");
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void ValuesEqualWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase);

        context.ShouldHaveSingleError("value", $"value must not be {Formatter.Format(comparativeValue)}.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValuesNotEqualWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void CustomErrorMessageWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase, "Errors.ValuesEqual");

        context.ShouldHaveSingleError("value", "Errors.ValuesEqual");
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(short value)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(value, (c, o) => $"{c.Key} must be different than {Formatter.Format(o)}");

        context.ShouldHaveSingleError("value", $"value must be different than {Formatter.Format(value)}");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value).IsNotEqualTo(comparativeValue, (_, _) => "Foo");

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void CustomMessageFactoryWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value)
               .IsNotEqualTo(comparativeValue,
                             StringComparer.OrdinalIgnoreCase,
                             (c, o) => $"{c.Key} must be different than {Formatter.Format(o)}");

        context.ShouldHaveSingleError("value", $"value must be different than {Formatter.Format(comparativeValue)}");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorsWithCustomMessageFactoryAndCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        context.Check(dto.Value)
               .IsNotEqualTo(comparativeValue,
                             StringComparer.OrdinalIgnoreCase,
                             (_, _) => "Foo");

        context.ShouldHaveNoError();
    }
}