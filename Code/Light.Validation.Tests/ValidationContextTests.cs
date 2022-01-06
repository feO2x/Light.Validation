using System;
using FluentAssertions;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests;

public static class ValidationContextTests
{
    [Fact]
    public static void SupplyAndRetrieveDerivedOptions()
    {
        var myOptions = new MyOptions();
        var context = new ValidationContext(myOptions);

        var retrievedOptions = context.GetOptionsAs<MyOptions>();

        retrievedOptions.Should().BeSameAs(myOptions);
    }

    [Fact]
    public static void InvalidOptionsCast()
    {
        var act = () => new ValidationContext().GetOptionsAs<MyOptions>();

        act.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public static void SupplyAndRetrieveDerivedErrorTemplates()
    {
        var errorTemplates = new MyErrorTemplates();
        var context = new ValidationContext(errorTemplates: errorTemplates);

        var retrievedTemplates = context.GetErrorTemplatesAs<MyErrorTemplates>();

        retrievedTemplates.Should().BeSameAs(errorTemplates);
    }

    [Fact]
    public static void InvalidErrorTemplatesCast()
    {
        var act = () => new ValidationContext().GetErrorTemplatesAs<MyErrorTemplates>();

        act.Should().Throw<InvalidCastException>();
    }

    private sealed record MyOptions : ValidationContextOptions;

    private sealed record MyErrorTemplates : ErrorTemplates;
}