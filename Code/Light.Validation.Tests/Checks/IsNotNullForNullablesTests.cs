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

        context.Check(dto.NullableValue).IsNotNull();

        context.ShouldHaveSingleError("nullableValue", "nullableValue must not be null.");
    }
    
    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NotNull(int validValue)
    {
        var dto = new Dto { NullableValue = validValue };
        var context = new ValidationContext();

        context.Check(dto.NullableValue).IsNotNull();

        context.ShouldHaveNoError();
    }
    
    [Fact]
    public static void CustomErrorMessage()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.NullableValue).IsNotNull("How can you pass null?");

        context.ShouldHaveSingleError("nullableValue", "How can you pass null?");
    }
    
    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.NullableValue).IsNotNull(c => $"Damn you, {c.Key} is null!");

        context.ShouldHaveSingleError("nullableValue", "Damn you, nullableValue is null!");
    }
    
    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(int validValue)
    {
        var dto = new Dto { NullableValue = validValue };
        var context = new ValidationContext();

        context.Check(dto.NullableValue).IsNotNull(_ => "It doesn't matter");

        context.ShouldHaveNoError();
    }
    
    private sealed class Dto
    {
        public int? NullableValue { get; init; }
    }
}