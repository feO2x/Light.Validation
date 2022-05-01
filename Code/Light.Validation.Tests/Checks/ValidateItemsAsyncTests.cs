using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class ValidateItemsAsyncTests
{
    [Fact]
    public static async Task AllItemsValid()
    {
        var array = new[] { 15, 20, 25 };
        var context = ValidationContextFactory.CreateContext();

        var check = await context.Check(array)
                                 .ValidateItemsAsync((Check<int> number) =>
                                  {
                                      number.IsLessThan(30);
                                      return Task.FromResult(number);
                                  });

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static async Task SomeItemsInvalid()
    {
        var list = new List<int> { 11, 42, 3813 };
        var context = ValidationContextFactory.CreateContext();

        var check = await context.Check(list)
                                 .ValidateItemsAsync((Check<int> number) =>
                                  {
                                      number.IsLessThan(100);
                                      return Task.FromResult(number);
                                  });

        context.ShouldHaveSingleComplexError(
            "list",
            new Dictionary<string, object>
            {
                ["2"] = "The value must be less than 100"
            }
        );
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static async Task NoErrorWithShortCircuitedCheck()
    {
        var list = new List<int> { -4, 11, 5 };
        var context = ValidationContextFactory.CreateContext();

        var check = await context.Check(list)
                                 .ShortCircuit()
                                 .ValidateItemsAsync((Check<int> number) =>
                                  {
                                      number.IsLessThan(5);
                                      return Task.FromResult(number);
                                  });

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static async Task ShortCircuitOnError()
    {
        var list = new List<int> { 10, 20, 30 };
        var context = ValidationContextFactory.CreateContext();

        var check = await context.Check(list)
                                 .ValidateItemsAsync((Check<int> number) =>
                                                     {
                                                         number.IsGreaterThan(25);
                                                         return Task.FromResult(number);
                                                     },
                                                     shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static async Task NoShortCircuitWhenAllItemsValid()
    {
        var array = new[] { 15, 20, 25 };
        var context = ValidationContextFactory.CreateContext();

        var check = await context.Check(array)
                                 .ValidateItemsAsync((Check<int> number) =>
                                                     {
                                                         number.IsLessThan(30);
                                                         return Task.FromResult(number);
                                                     },
                                                     shortCircuitOnError: true);

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static async Task AutomaticNullCheck()
    {
        List<int> list = null!;
        var context = ValidationContextFactory.CreateContext();

        var check = await context.Check(list)
                                 .ValidateItemsAsync((Check<int> number) =>
                                  {
                                      number.IsGreaterThan(10);
                                      return Task.FromResult(number);
                                  });

        context.ShouldHaveSingleError("list", "list must not be null");
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static async Task DisableAutomaticNullCheck()
    {
        List<string> list = null!;
        var context = ValidationContextFactory.CreateContext();

        var act = () => context.Check(list)
                               .ValidateItemsAsync((Check<string> text) =>
                                                   {
                                                       text.IsLongerThan(10);
                                                       return Task.FromResult(text);
                                                   },
                                                   isNullCheckingEnabled: false)
                               .AsTask();

        await act.Should().ThrowAsync<NullReferenceException>();
    }

    [Fact]
    public static async Task ShortCircuitOnNullCheck()
    {
        string[] array = null!;
        var context = ValidationContextFactory.CreateContext();

        var check = await context.Check(array)
                                 .ValidateItemsAsync((Check<string> text) =>
                                                     {
                                                         text.IsLongerThan(15);
                                                         return Task.FromResult(text);
                                                     },
                                                     shortCircuitOnError: true);

        context.ShouldHaveSingleError("array", "array must not be null");
        check.ShouldBeShortCircuited();
    }
}