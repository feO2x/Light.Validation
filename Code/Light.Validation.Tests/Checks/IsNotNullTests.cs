using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsNotNullTests
{
    public static readonly TheoryData<string> ValidValues =
        new ()
        {
            "Foo",
            "Bar",
            "c",
            "",
            " ",
            "\t\r\n"
        };

    [Fact]
    public static void ValueIsNull()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsNotNull();

        context.ShouldHaveSingleError("someValue", "someValue must not be null.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NotNull(string validString)
    {
        var dto = new Dto { SomeValue = validString };
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsNotNull();

        context.ShouldHaveNoError();
    }

    [Fact]
    public static void CustomErrorMessage()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsNotNull("How can you pass null?");

        context.ShouldHaveSingleError("someValue", "How can you pass null?");
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsNotNull(c => $"Damn you, {c.Key} is null!");

        context.ShouldHaveSingleError("someValue", "Damn you, someValue is null!");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string validValue)
    {
        var dto = new Dto { SomeValue = validValue };
        var context = new ValidationContext();

        context.Check(dto.SomeValue).IsNotNull(_ => "It doesn't matter");

        context.ShouldHaveNoError();
    }

    private sealed class Dto
    {
        public string SomeValue { get; init; } = null!;
    }
}