using System;
using System.Collections.Generic;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;
using Range = Light.Validation.Tools.Range;

namespace Light.Validation.Tests.Checks;

public static class HasCountInTests
{
    public static readonly TheoryData<int[]?, Range<int>> InvalidValues =
        new ()
        {
            { null, Range.FromInclusive(2).ToInclusive(5) },
            { new []{ 1, 2, 3 }, Range.FromExclusive(3).ToExclusive(10) },
            { Array.Empty<int>(), Range.FromInclusive(1).ToInclusive(100) },
            { new []{1, 2, 3, 4, 5}, Range.FromInclusive(0).ToExclusive(5) }
        };

    public static readonly TheoryData<List<string>, Range<int>> ValidValues =
        new ()
        {
            { new (){ "Foo", "Bar", "Baz" }, Range.FromInclusive(1).ToInclusive(5) },
            { new (){ "Foo" }, Range.FromInclusive(1).ToInclusive(10) },
            { new (){ "Foo", "Bar" }, Range.FromInclusive(0).ToExclusive(3) }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidRanges(int[] array, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCountIn(range);

        context.ShouldHaveSingleError("Value", $"Value must have {range.CreateRangeDescriptionText()} items");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidRanges(List<string> list, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasCountIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(int[] array, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCountIn(range, "Custom Error Message");

        context.ShouldHaveSingleError("Value", "Custom Error Message");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitOnError(int[] array, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCountIn(range, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(int[] array, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasCountIn(range);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(int[] array, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCountIn(range, (c, r) => $"{c.DisplayName} should have {r.CreateRangeDescriptionText()} elements");

        context.ShouldHaveSingleError("Value", $"Value should have {range.CreateRangeDescriptionText()} elements");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(List<string> list, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasCountIn(range, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(int[] array, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCountIn(range, (_, _) => "The error", shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(int[] array, Range<int> range)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasCountIn(range, (_, _) => "nothing to see here");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}