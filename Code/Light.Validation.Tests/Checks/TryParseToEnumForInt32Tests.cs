using System;
using FluentAssertions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class TryParseToEnumForInt32Tests
{
    public static readonly TheoryData<int> InvalidValues =
        new () { -1, 14, -3262 };

    public static readonly TheoryData<int, ConsoleColor> ValidValues =
        new ()
        {
            { 0, ConsoleColor.Black },
            { 3, ConsoleColor.DarkCyan },
            { 7, ConsoleColor.Gray },
            { 15, ConsoleColor.White }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidValue(int value)
    {
        var (dto, context) = Test.SetupDefault(value);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleModifiers>(out var parsedValue);

        result.Should().BeFalse();
        context.ShouldHaveSingleError("Value", "Value must be one of the allowed values");
        parsedValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidValue(int value, ConsoleColor expectedEnumValue)
    {
        var (dto, context) = Test.SetupDefault(value);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleColor>(out var parsedValue);

        result.Should().BeTrue();
        context.ShouldHaveNoErrors();
        parsedValue.Should().Be(expectedEnumValue);
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessage(int invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleModifiers>(out var parsedValue, "This is wrong!");

        result.Should().BeFalse();
        context.ShouldHaveSingleError("Value", "This is wrong!");
        parsedValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(int invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value)
                            .ShortCircuit()
                            .TryParseToEnum<ConsoleModifiers>(out var parsedValue, "This is wrong!");

        result.Should().BeFalse();
        context.ShouldHaveNoErrors();
        parsedValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(int invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleModifiers>(out var parsedValue, c => $"{c.Key} is stupidly wrong");

        result.Should().BeFalse();
        context.ShouldHaveSingleError("Value", "Value is stupidly wrong");
        parsedValue.Should().Be(default);
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidValueWithCustomMessageFactory(int validValue, ConsoleColor expectedValue)
    {
        var (dto, context) = Test.SetupDefault(validValue);

        var result = context.Check(dto.Value).TryParseToEnum<ConsoleColor>(out var parsedValue, _ => "whatever");

        result.Should().BeTrue();
        context.ShouldHaveNoErrors();
        parsedValue.Should().Be(expectedValue);
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(int invalidValue)
    {
        var (dto, context) = Test.SetupDefault(invalidValue);

        var result = context.Check(dto.Value)
                            .ShortCircuit()
                            .TryParseToEnum<ConsoleModifiers>(out var parsedValue, _ => "whatever");

        result.Should().BeFalse();
        context.ShouldHaveNoErrors();
        parsedValue.Should().Be(default);
    }
}