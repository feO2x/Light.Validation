using System;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Xunit;

namespace Light.Validation.Tests.Checks;

public static class GuidIsNotEmptyTests
{
    public static readonly TheoryData<Guid> ValidGuids = 
        new ()
        {
            Guid.Parse("F25BCBB0-7D2D-4810-BE76-D3F8EB400D7B"),
            Guid.Parse("75873504-540D-4B1F-ABE4-CC01D96FA273"),
            Guid.Parse("7172B4C7-C918-45A0-9387-AFCEA456B851")
        };

    [Fact]
    public static void EmptyGuid()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.Id).IsNotEmpty();

        context.ShouldHaveSingleError("id", "id must not be an empty GUID.");
    }

    [Theory]
    [MemberData(nameof(ValidGuids))]
    public static void ValidGuid(Guid guid)
    {
        var dto = new Dto { Id = guid };
        var context = new ValidationContext();

        context.Check(dto.Id).IsNotEmpty();

        context.ShouldHaveNoError();
    }

    [Fact]
    public static void CustomErrorMessage()
    {
        var dto = new Dto();
        var context = new ValidationContext();
        
        context.Check(dto.Id).IsNotEmpty("An empty GUID? Are you kidding?");

        context.ShouldHaveSingleError("id", "An empty GUID? Are you kidding?");
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto();
        var context = new ValidationContext();

        context.Check(dto.Id).IsNotEmpty(c => $"The {c.Key} is empty");

        context.ShouldHaveSingleError("id", "The id is empty");
    }

    [Theory]
    [MemberData(nameof(ValidGuids))]
    public static void NoErrorWithCustomMessageFactory(Guid validGuid)
    {
        var dto = new Dto { Id = validGuid };
        var context = new ValidationContext();

        context.Check(dto.Id).IsNotEmpty(_ => "whatever");

        context.ShouldHaveNoError();
    }

    private sealed class Dto
    {
        public Guid Id { get; init; } = Guid.Empty;
    }
}