using System.Collections.Generic;
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
}