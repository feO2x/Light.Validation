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
            { " ", null! },
            { null!, "\t" }
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

        var check = context.Check(dto.Value).IsNotEqualTo(value);

        context.ShouldHaveSingleError("Value", $"Value must not be {Formatter.Format(value)}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValuesNotEqualWithDefaultEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(comparativeValue);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(short value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(value, "WTF");

        context.ShouldHaveSingleError("Value", "WTF");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(short value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(value, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorMessageForShortCircuitedCheck(short value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsNotEqualTo(value);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void ValuesEqualWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase);

        context.ShouldHaveSingleError("Value", $"Value must not be {Formatter.Format(comparativeValue)}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValuesNotEqualWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void CustomErrorMessageWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase, "Errors.ValuesEqual");

        context.ShouldHaveSingleError("Value", "Errors.ValuesEqual");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void ShortCircuitWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void NoErrorsForShortCircuitedCheckWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsNotEqualTo(comparativeValue, StringComparer.OrdinalIgnoreCase);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(short value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(value, (c, o) => $"{c.Key} must be different than {Formatter.Format(o)}");

        context.ShouldHaveSingleError("Value", $"Value must be different than {Formatter.Format(value)}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value).IsNotEqualTo(comparativeValue, (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void CustomMessageFactoryWithCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .IsNotEqualTo(comparativeValue,
                                         StringComparer.OrdinalIgnoreCase,
                                         (c, o) => $"{c.Key} must be different than {Formatter.Format(o)}");

        context.ShouldHaveSingleError("Value", $"Value must be different than {Formatter.Format(comparativeValue)}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorsWithCustomMessageFactoryAndCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .IsNotEqualTo(comparativeValue,
                                         StringComparer.OrdinalIgnoreCase,
                                         (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(IgnoreCaseInvalidValues))]
    public static void NoErrorsForShortCircuitedCheckWithCustomMessageFactoryAndCustomEqualityComparer(string value, string comparativeValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .IsNotEqualTo(comparativeValue,
                                         StringComparer.OrdinalIgnoreCase,
                                         (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}