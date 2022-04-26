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
        var context = CreateContext(myOptions);

        var retrievedOptions = context.GetOptionsAs<MyOptions>();

        retrievedOptions.Should().BeSameAs(myOptions);
    }

    [Fact]
    public static void InvalidOptionsCast()
    {
        var act = () => CreateContext().GetOptionsAs<MyOptions>();

        act.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public static void SupplyAndRetrieveDerivedErrorTemplates()
    {
        var errorTemplates = new MyErrorTemplates();
        var context = CreateContext(errorTemplates: errorTemplates);

        var retrievedTemplates = context.GetErrorTemplatesAs<MyErrorTemplates>();

        retrievedTemplates.Should().BeSameAs(errorTemplates);
    }

    [Fact]
    public static void InvalidErrorTemplatesCast()
    {
        var act = () => CreateContext().GetErrorTemplatesAs<MyErrorTemplates>();

        act.Should().Throw<InvalidCastException>();
    }

    public static ValidationContext CreateContext(ValidationContextOptions? options = null, ErrorTemplates? errorTemplates = null) =>
        new ValidationContextFactory(options ?? ValidationContextOptions.Default,
                                     errorTemplates ?? ErrorTemplates.Default)
           .CreateValidationContext();

    private sealed class MyOptions : ValidationContextOptions { }

    private sealed class MyErrorTemplates : ErrorTemplates { }
}