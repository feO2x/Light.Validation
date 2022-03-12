using FluentAssertions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsNotNullForNullablesTests
{
    public static readonly TheoryData<int> ValidValues =
        new ()
        {
            42,
            1,
            0,
            -1,
            int.MinValue,
            int.MaxValue
        };
    
    [Fact]
    public static void ValueIsNull()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        var check = context.Check(dto.NullableValue).IsNotNull();

        context.ShouldHaveSingleError("nullableValue", "nullableValue must not be null");
        check.ShouldBeShortCircuited();
    }
    
    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NotNull(int validValue)
    {
        var dto = new Dto { NullableValue = validValue };
        var context = new ValidationContext();

        var check = context.Check(dto.NullableValue).IsNotNull();

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }
    
    [Fact]
    public static void CustomErrorMessage()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        var check = context.Check(dto.NullableValue).IsNotNull("How can you pass null?");

        context.ShouldHaveSingleError("nullableValue", "How can you pass null?");
        check.ShouldBeShortCircuited();
    }
    
    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        var check = context.Check(dto.NullableValue).IsNotNull(c => $"Damn you, {c.Key} is null!");

        context.ShouldHaveSingleError("nullableValue", "Damn you, nullableValue is null!");
        check.ShouldBeShortCircuited();
    }
    
    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(int validValue)
    {
        var dto = new Dto { NullableValue = validValue };
        var context = new ValidationContext();

        var check = context.Check(dto.NullableValue).IsNotNull(_ => "It doesn't matter");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void DisableShortCircuiting()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        var check = context.Check(dto.NullableValue).IsNotNull(shortCircuitOnError: false);

        context.ShouldHaveErrors();
        check.IsShortCircuited.Should().BeFalse();
    }

    [Fact]
    public static void DisableShortCircuitingWithCustomMessageFactory()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        var check = context.Check(dto.NullableValue).IsNotNull(_ => "OMG it's null", false);

        context.ShouldHaveErrors();
        check.ShouldNotBeShortCircuited();
    }
    
    private sealed class Dto
    {
        public int? NullableValue { get; init; }
    }
}