using System;
using System.Collections.Generic;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class HasMinimumCountTests
{
    public static readonly TheoryData<int[]?, int> InvalidValues =
        new ()
        {
            { null, 1 },
            { Array.Empty<int>(), 1 },
            { new []{ 1, 2, 3 }, 4 },
            { new []{ 1, 2, 3, 4, 5 }, 7 }
        };

    public static readonly TheoryData<List<string>, int> ValidValues =
        new ()
        {
            { new (), 0 },
            { new () { "Foo" }, 1 },
            { new () { "Foo", "Bar", "Baz" }, 2 },
            { new () { "Foo", "Bar", "Baz" }, 3 },
        };

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void InvalidMinimumCount(int[] array, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMinimumCount(minimumCount);

        context.ShouldHaveSingleError("Value", $"Value must have at least {minimumCount} {(minimumCount == 1 ? "item" : "items")}");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void ValidMinimumCount(List<string> list, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasMinimumCount(minimumCount);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomErrorMessage(int[] array, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMinimumCount(minimumCount, "Errors.MinimumCount");
        
        context.ShouldHaveSingleError("Value", "Errors.MinimumCount");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuit(int[] array, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMinimumCount(minimumCount, shortCircuitOnError: true);
        
        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }
    
    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void NoErrorOnShortCircuitedCheck(int[] array, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value)
                           .ShortCircuit()
                           .HasMinimumCount(minimumCount);

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
    
    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void CustomMessageFactory(int[] array, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMinimumCount(minimumCount, (c, l) => $"{c.Key} must have at least {l} items");

        context.ShouldHaveSingleError("Value", $"Value must have at least {minimumCount} items");
        check.ShouldNotBeShortCircuited();
    }
    
    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(List<string> list, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(list);

        var check = context.Check(dto.Value).HasMinimumCount(minimumCount, (_, _) => "Foo");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }
    
    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ShortCircuitWithCustomMessageFactory(int[] array, int minimumCount)
    {
        var (dto, context) = Test.SetupDefault(array);

        var check = context.Check(dto.Value).HasMinimumCount(minimumCount, (_, _) => "My Error Message", true);

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
                           .HasMinimumCount(count, (_, _) => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }
}