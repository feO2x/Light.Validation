using System.Collections.Generic;
using System.Threading.Tasks;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Checks;

public sealed class ValidateWithAsyncTests
{
    public ValidateWithAsyncTests()
    {
        Context = ValidationContextFactory.CreateContext();
        Validator = new AsyncChildDtoValidator(ValidationContextFactory.Instance);
    }

    private ValidationContext Context { get; }
    private AsyncChildDtoValidator Validator { get; }

    [Fact]
    public async Task ChildObjectValid()
    {
        var dto = new ComplexDto { Child = new () { Id = 30, Name = "John Doe" } };

        var check = await Context.Check(dto.Child).ValidateWithAsync(Validator);

        Context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public async Task ChildObjectInvalid()
    {
        var dto = new ComplexDto { Child = new () { Id = -3, Name = null! } };

        var check = await Context.Check(dto.Child).ValidateWithAsync(Validator);

        Context.ShouldHaveSingleComplexError(
            "Child",
            new Dictionary<string, object>
            {
                ["Id"] = "Id must be greater than 0",
                ["Name"] = "Name must have 2 (inclusive) to 100 (inclusive) characters"
            }
        );
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public async Task NoErrorOnShortCircuitedCheck()
    {
        var dto = new ComplexDto { Child = new () { Id = -1, Name = "Foo" } };

        var check = await Context.Check(dto.Child)
                                 .ShortCircuit()
                                 .ValidateWithAsync(Validator);

        Context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public async Task ShortCircuit()
    {
        var dto = new ComplexDto { Child = new() { Id = -1, Name = "Foo" } };

        var check = await Context.Check(dto.Child)
                                 .ValidateWithAsync(Validator, shortCircuitOnError: true);

        Context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public async Task NoShortCircuitWithValidChild()
    {
        var dto = new ComplexDto { Child = new () { Id = 43, Name = "James Blow" } };

        var check = await Context.Check(dto.Child)
                                 .ValidateWithAsync(Validator, shortCircuitOnError: true);

        Context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }
}

public sealed class AsyncChildDtoValidator : AsyncValidator<ChildDto>
{
    public AsyncChildDtoValidator(ValidationContextFactory validationContextFactory) :
        base(validationContextFactory) { }

    protected override Task<ChildDto> PerformValidationAsync(ValidationContext context, ChildDto dto)
    {
        context.Check(dto.Id).IsGreaterThan(0);
        dto.Name = context.Check(dto.Name).HasLengthIn(Range.FromInclusive(2).ToInclusive(100));
        return Task.FromResult(dto);
    }
}