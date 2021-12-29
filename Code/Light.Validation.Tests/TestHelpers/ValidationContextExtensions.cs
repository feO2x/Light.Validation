using System.Collections.Generic;
using FluentAssertions;

namespace Light.Validation.Tests.TestHelpers;

public static class ValidationContextExtensions
{
    public static ValidationContext ShouldHaveSingleError(this ValidationContext context, string key, string message)
    {
        context.Errors.Should().Equal(new[] { new KeyValuePair<string, object>(key, message) });
        return context;
    }

    public static ValidationContext ShouldHaveNoError(this ValidationContext context)
    {
        context.Errors.Should().BeNull();
        return context;
    }
}