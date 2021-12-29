using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class IsNotNullForReferencesTests
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

        context.Check(dto.ReferenceValue).IsNotNull();

        context.ShouldHaveSingleError("referenceValue", "referenceValue must not be null.");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NotNull(string validString)
    {
        var dto = new Dto { ReferenceValue = validString };
        var context = new ValidationContext();

        context.Check(dto.ReferenceValue).IsNotNull();

        context.ShouldHaveNoError();
    }

    [Fact]
    public static void CustomErrorMessage()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.ReferenceValue).IsNotNull("How can you pass null?");

        context.ShouldHaveSingleError("referenceValue", "How can you pass null?");
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.ReferenceValue).IsNotNull(c => $"Damn you, {c.Key} is null!");

        context.ShouldHaveSingleError("referenceValue", "Damn you, referenceValue is null!");
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public static void NoErrorWithCustomMessageFactory(string validValue)
    {
        var dto = new Dto { ReferenceValue = validValue };
        var context = new ValidationContext();

        context.Check(dto.ReferenceValue).IsNotNull(_ => "It doesn't matter");

        context.ShouldHaveNoError();
    }

    private sealed class Dto
    {
        public string ReferenceValue { get; init; } = null!;
    }
}