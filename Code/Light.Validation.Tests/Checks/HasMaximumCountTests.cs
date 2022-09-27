using System.Collections.Generic;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class HasMaximumCountTests
{
    public static readonly TheoryData<int[]?, int> InvalidValues =
        new ()
        {
            { new []{ 1, 2, 3 }, 2 },
            { new []{ 1, 2, 3 }, 1 },
            { new []{ 1, 2, 3, 4, 5 }, 4 },
            { new []{ 1 }, 0 }
        };

    public static readonly TheoryData<List<string>, int> ValidValues =
        new ()
        {
            { new () { "Foo" }, 1 },
            { new () { "Foo", "Bar", "Baz" }, 3 },
            { new (), 0 }
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidMaximumCount(int[] array, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMaximumCount(maximumCount);

        context.ShouldHaveSingleError("Value", $"Value must have at most {maximumCount} {(maximumCount == 1 ? "item" : "items")}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidMaximumCount(List<string> list, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasMaximumCount(maximumCount);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(int[] array, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMaximumCount(maximumCount, "Errors.MaximumCount");

        context.ShouldHaveSingleError("Value", "Errors.MaximumCount");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(int[] array, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMaximumCount(maximumCount, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(int[] array, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasMaximumCount(maximumCount);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(int[] array, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMaximumCount(maximumCount, (c, l) => $"{c.DisplayName} must have {l} items at most");

        context.ShouldHaveSingleError("Value", $"Value must have {maximumCount} items at most");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(List<string> list, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasMaximumCount(maximumCount, (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(int[] array, int maximumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMaximumCount(maximumCount, (_, _) => "The error", shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheckWithCustomMessageFactory(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasMaximumCount(count, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}