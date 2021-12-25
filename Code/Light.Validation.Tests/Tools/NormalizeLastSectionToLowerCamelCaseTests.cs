using FluentAssertions;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Tools;

public static class NormalizeLastSectionToLowerCamelCaseTests
{
    [Theory]
    [InlineData("A", "a")]
    [InlineData("G", "g")]
    [InlineData("Ä", "ä")]
    [InlineData("Ü", "ü")]
    [InlineData("Ö", "ö")]
    [InlineData("SomeProperty", "someProperty")]
    [InlineData("dto.MyProperty", "myProperty")]
    [InlineData("dto.User.Address", "address")]
    [InlineData("PropertyWith.", "propertyWith.")]
    public static void ChangeKeyDuringNormalization(string inputKey, string expectedKey)
    {
        var normalizedKey = inputKey.NormalizeLastSectionToLowerCamelCase();

        normalizedKey.Should().Be(expectedKey);
    }

    [Theory]
    [InlineData("")]
    [InlineData(".")]
    [InlineData("-")]
    [InlineData(";")]
    [InlineData("_")]
    [InlineData("\"")]
    [InlineData("!")]
    [InlineData("§")]
    [InlineData("%")]
    [InlineData("&")]
    [InlineData("/")]
    [InlineData("(")]
    [InlineData(")")]
    [InlineData("=")]
    [InlineData("{")]
    [InlineData("}")]
    [InlineData("[")]
    [InlineData("]")]
    [InlineData("?")]
    [InlineData("ß")]
    [InlineData("°")]
    [InlineData("^")]
    [InlineData("`")]
    [InlineData("´")]
    [InlineData("#")]
    [InlineData("'")]
    [InlineData("*")]
    [InlineData("+")]
    [InlineData("~")]
    [InlineData("<")]
    [InlineData(">")]
    [InlineData("|")]
    [InlineData("µ")]
    [InlineData("@")]
    [InlineData("€")]
    [InlineData("ä")]
    [InlineData("ü")]
    [InlineData("ö")]
    public static void ReturnSameKeyWhenNormalizationIsNotNecessary(string inputKey)
    {
        var normalizedKey = inputKey.NormalizeLastSectionToLowerCamelCase();

        normalizedKey.Should().BeSameAs(inputKey);
    }
}