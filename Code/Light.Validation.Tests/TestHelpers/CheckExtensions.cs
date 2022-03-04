using FluentAssertions;

namespace Light.Validation.Tests.TestHelpers;

public static class CheckExtensions
{
    public static void ShouldBeShortCircuited<T>(this Check<T> check) =>
        check.IsShortCircuited.Should().BeTrue();
    
    public static void ShouldNotBeShortCircuited<T>(this Check<T> check) =>
        check.IsShortCircuited.Should().BeFalse();
}