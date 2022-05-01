using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class ValidateItemsTests
{
    [Fact]
    public static void AllItemsValid()
    {
        var list = new List<int> { 11, 42, 3813 };
        var context = ValidationContextFactory.CreateContext();

        var check = context.Check(list).ValidateItems((Check<int> number) => number.IsGreaterThan(10));

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void SomeItemsInvalid()
    {
        var array = new [] { 4, 12, 8, 28918, 30 };
        var context = ValidationContextFactory.CreateContext();

        var check = context.Check(array).ValidateItems((Check<int> number) => number.IsGreaterThanOrEqualTo(10));

        context.ShouldHaveSingleComplexError(
            "array",
            new ()
            {
                ["0"] = "The value must be greater than or equal to 10",
                ["2"] = "The value must be greater than or equal to 10"
            });
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void NoErrorWithShortCircuitedCheck()
    {
        var list = new List<int> { -4, 11, 5 };
        var context = ValidationContextFactory.CreateContext();

        var check = context.Check(list)
                           .ShortCircuit()
                           .ValidateItems((Check<int> number) => number.IsGreaterThan(10));

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void ShortCircuitOnError()
    {
        var list = new List<int> { 3, 12, 9 };
        var context = ValidationContextFactory.CreateContext();

        var check = context.Check(list)
                           .ValidateItems((Check<int> number) => number.IsGreaterThanOrEqualTo(5), shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void NoShortCircuitWhenAllItemsValid()
    {
        var list = new List<int> { 11, 42, 3813 };
        var context = ValidationContextFactory.CreateContext();

        var check = context.Check(list).ValidateItems((Check<int> number) => number.IsGreaterThan(10), shortCircuitOnError: true);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void AutomaticNullCheck()
    {
        List<string> list = null!;
        var context = ValidationContextFactory.CreateContext();

        var check = context.Check(list)
                           .ValidateItems((Check<string> text) => text.IsLongerThan(5));

        context.ShouldHaveSingleError("list", "list must not be null");
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void DisableAutomaticNullCheck()
    {
        List<object> list = null!;
        var context = ValidationContextFactory.CreateContext();

        var act = () => context.Check(list)
                               .ValidateItems((Check<object> value) => value.IsNotNull(), isNullCheckingEnabled: false);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public static void ShortCircuitOnNullCheck()
    {
        List<string> list = null!;
        var context = ValidationContextFactory.CreateContext();

        var check = context.Check(list)
                           .ValidateItems((Check<string> text) => text.IsNotNull(), shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }
}