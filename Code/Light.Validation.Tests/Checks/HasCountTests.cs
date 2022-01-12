using System.Collections.Generic;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class HasCountTests
{
    public static readonly TheoryData<int[], int> InvalidValues =
        new ()
        {
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

        context.Check(dto.Value).HasCount(count);

        context.ShouldHaveSingleError("value", $"value must have {count} {(count == 1 ? "item" : "items")}.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void CollectionWithValidCount(List<int> list)
    {
        var (dto, context) = Test.SetupDefault(list);

        context.Check(dto.Value).HasCount(list.Count);

        context.ShouldHaveNoError();
    }

    [Fact]
    public static void NoErrorWhenNull()
    {
        var (dto, context) = Test.SetupDefault(default(IList<string>)!);

        context.Check(dto.Value).HasCount(50);

        context.ShouldHaveNoError();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        context.Check(dto.Value).HasCount(count, "Errors.InvalidCollectionCount");

        context.ShouldHaveSingleError("value", "Errors.InvalidCollectionCount");
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(int[] array, int count)
    {
        var (dto, context) = Test.SetupDefault(array);

        context.Check(dto.Value).HasCount(count, (c, l) => $"{c.Key} must have length {l}");

        context.ShouldHaveSingleError("value", $"value must have length {count}");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(List<int> list)
    {
        var (dto, context) = Test.SetupDefault(list);

        context.Check(dto.Value).HasCount(list.Count, (_, _) => "Foo");

        context.ShouldHaveNoError();
    }
}