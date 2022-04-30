using FluentAssertions;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Tools;

public static class GetSectionAfterLastDotTests
{
    [Theory]
    [InlineData("A", "A")]
    [InlineData("G", "G")]
    [InlineData("Ä", "Ä")]
    [InlineData("Ü", "Ü")]
    [InlineData("Ö", "Ö")]
    [InlineData("SomeProperty", "SomeProperty")]
    [InlineData("dto.MyProperty", "MyProperty")]
    [InlineData("dto.User.Address", "Address")]
    [InlineData("PropertyWith.", "PropertyWith.")]
    [InlineData("  dto.LastName ", "LastName")]
    public static void ChangeKeyDuringNormalization(string inputKey, string expectedKey)
    {
        var normalizedKey = inputKey.GetSectionAfterLastDot();

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
    [InlineData("Foo")]
    public static void ReturnSameKeyWhenNormalizationIsNotNecessary(string inputKey)
    {
        var normalizedKey = inputKey.GetSectionAfterLastDot();

        normalizedKey.Should().BeSameAs(inputKey);
    }
}