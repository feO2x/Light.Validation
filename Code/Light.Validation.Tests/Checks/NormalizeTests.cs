using FluentAssertions;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class NormalizeTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\t")]
    [InlineData(" \r\n")]
    public static void NormalizeNullEmptyOrWhiteSpace(string input)
    {
        var (dto, context) = Test.SetupDefault(input);

        var check = context.Check(dto.Value).Normalize();

        check.Value.Should().BeSameAs(string.Empty);
    }

    [Theory]
    [InlineData("Foo", "Foo")]
    [InlineData(" Bar\n", "Bar")]
    [InlineData("baz ", "baz")]
    public static void NormalizeNonEmpty(string input, string expected)
    {
        var (dto, context) = Test.SetupDefault(input);

        var check = context.Check(dto.Value).Normalize();

        check.Value.Should().Be(expected);
    }
}