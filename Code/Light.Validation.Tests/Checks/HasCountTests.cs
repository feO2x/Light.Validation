using System.Collections.Generic;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class HasCountTests
{
    public static readonly TheoryData<int[]?, int> InvalidValues =
        new ()
        {
            { null, 3 },
            { new []{ 1, 2, 3 }, 2 },
            { new []{ 1, 2, 3, 4 }, 1 },
            { new []{ 1, 2 }, 10 },
            { new []{ 1, 2 }, 0 }
        };

    public static readonly TheoryData<List<int>> ValidValues =
        new ()
        {
            new List<int> { 1, 2, 5, 9 },
            new List<int> { 0, 1 },
            new List<int>()
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CollectionWithDifferentCount(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCount(count);

        context.ShouldHaveSingleError("Value", $"Value must have {count} {(count == 1 ? "item" : "items")}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void CollectionWithValidCount(List<int> list)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasCount(list.Count);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCount(count, "Errors.InvalidCollectionCount");

        context.ShouldHaveSingleError("Value", "Errors.InvalidCollectionCount");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCount(count, shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasCount(count);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCount(count, (c, l) => $"{c.Key} must have length {l}");

        context.ShouldHaveSingleError("Value", $"Value must have length {count}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(List<int> list)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasCount(list.Count, (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasCount(count, (_, _) => "My Error Message", true);

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
                           .HasCount(count, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}