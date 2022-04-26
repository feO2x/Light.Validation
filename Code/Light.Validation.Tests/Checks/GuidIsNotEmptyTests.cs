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
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id).IsNotEmpty();

        context.ShouldHaveSingleError("id", "id must not be an empty GUID");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidGuids))]
    public static void ValidGuid(Guid guid)
    {
        var dto = new Dto { Id = guid };
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id).IsNotEmpty();

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void ShortCircuit()
    {
        var dto = new Dto();
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id).IsNotEmpty(shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void NoErrorOnShortCircuitedCheck()
    {
        var dto = new Dto();
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id)
                           .ShortCircuit()
                           .IsNotEmpty();

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void CustomErrorMessage()
    {
        var dto = new Dto();
        var context = ValidationContextFactory.CreateDefaultContext();
        
        var check = context.Check(dto.Id).IsNotEmpty("An empty GUID? Are you kidding?");

        context.ShouldHaveSingleError("id", "An empty GUID? Are you kidding?");
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void CustomErrorMessageFactory()
    {
        var dto = new Dto();
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id).IsNotEmpty(c => $"The {c.Key} is empty");

        context.ShouldHaveSingleError("id", "The id is empty");
        check.ShouldNotBeShortCircuited();
    }

    [Theory]
    [MemberData(nameof(ValidGuids))]
    public static void NoErrorWithCustomMessageFactory(Guid validGuid)
    {
        var dto = new Dto { Id = validGuid };
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id).IsNotEmpty(_ => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public static void ShortCircuitWithCustomMessageFactory()
    {
        var dto = new Dto();
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id).IsNotEmpty(c => $"{c.Key} must be a valid GUID", shortCircuitOnError: true);

        context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public static void NoErrorForShortCircuitedCheckWithCustomMessageFactory()
    {
        var dto = new Dto();
        var context = ValidationContextFactory.CreateDefaultContext();

        var check = context.Check(dto.Id)
                           .ShortCircuit()
                           .IsNotEmpty(_ => "whatever");

        context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    private sealed class Dto
    {
        public Guid Id { get; init; } = Guid.Empty;
    }
}