﻿using System;
using FluentAssertions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class TryParseToEnumTests
{
    public static readonly TheoryData<string> InvalidValues =
        new ()
        {
            "Foo",
            " bar ",
            "",
            null!
        };

    public static readonly TheoryData<string, ConsoleColor> ValidValues =
        new ()
        {
            { "Red", ConsoleColor.Red },
            { "black", ConsoleColor.Black },
            { "darkMagenta", ConsoleColor.DarkMagenta }
        };

    public static readonly TheoryData<string> CaseSensitiveInvalidValues =
        new ()
        {
            "black",
            "darkMagenta",
            "cYAN",
            "",
            null!
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidValue(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleModifiers>(out var parsedEnumValue);

        result.Should().BeFalse();
        context.ShouldHaveSingleError("value", "value must be one of the allowed values");
        parsedEnumValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidValue(string validValue, ConsoleColor expectedValue)
    {
        var (dto, context) = Test.SetupDefault(validValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleColor>(out var parsedEnumValue);

        result.Should().BeTrue();
        context.ShouldHaveNoErrors();
        parsedEnumValue.Should().Be(expectedValue);
    }

    [Theory]
    [MemberData(nameof(CaseSensitiveInvalidValues))]
    public static void EnableCaseSensitivity(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleColor>(out var parsedEnumValue, false);

        result.Should().BeFalse();
        context.ShouldHaveSingleError("value", "value must be one of the allowed values");
        parsedEnumValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleModifiers>(out var parsedValue, message: "InvalidEnumValue");

        result.Should().BeFalse();
        context.ShouldHaveSingleError("value", "InvalidEnumValue");
        parsedValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleModifiers>(out var parsedValue, c => $"{c.Key} must not be {c.Value}");

        result.Should().BeFalse();
        // ReSharper disable once ConstantConditionalAccessQualifier -- in one test case, invalidValue is actually null
        context.ShouldHaveSingleError("value", $"value must not be {invalidValue?.Trim()}");
        parsedValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string validValue, ConsoleColor expectedColor)
    {
        var (dto, context) = Test.SetupDefault(validValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleColor>(out var parsedEnumValue, _ => "whatever");

        result.Should().BeTrue();
        context.ShouldHaveNoErrors();
        parsedEnumValue.Should().Be(expectedColor);
    }

    [Theory]
    [MemberData(nameof(CaseSensitiveInvalidValues))]
    public static void CaseSensitivityWithCustomMessageFactory(string invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleColor>(out var parsedEnumValue, _ => "It's wrong!", false);

        result.Should().BeFalse();
        context.ShouldHaveErrors();
        parsedEnumValue.Should().Be(default);
    }
}