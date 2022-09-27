using System.Collections.Generic;
using Light.Validation.Checks;
using Light.Validation.Tests.TestHelpers;
using Light.Validation.Tools;
using Xunit;

namespace Light.Validation.Tests.Checks;

public sealed class ValidateWithTests
{
    public ValidateWithTests()
    {
        Context = ValidationContextFactory.CreateContext();
        Validator = new (ValidationContextFactory.Instance);
    }

    private ValidationContext Context { get; }
    private ChildDtoValidator Validator { get; }

    [Fact]
    public void ChildObjectValid()
    {
        var dto = new ComplexDto { Child = new () { Id = 30, Name = "John Doe" } };

        var check = Context.Check(dto.Child).ValidateWith(Validator);

        Context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public void ChildObjectInvalid()
    {
        var dto = new ComplexDto { Child = new () { Id = -1, Name = "" } };

        var check = Context.Check(dto.Child).ValidateWith(Validator);

        Context.ShouldHaveSingleComplexError(
            "Child",
            new Dictionary<string, object>
            {
                ["Id"] = "Id must be greater than 0",
                ["Name"] = "Name must have 2 to 100 characters"
            }
        );
        check.ShouldNotBeShortCircuited();
    }

    [Fact]
    public void NoErrorOnShortCircuitedCheck()
    {
        var dto = new ComplexDto { Child = new() { Id = -1, Name = "Foo" } };

        var check = Context.Check(dto.Child)
                           .ShortCircuit()
                           .ValidateWith(Validator);

        Context.ShouldHaveNoErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public void ShortCircuit()
    {
        var dto = new ComplexDto { Child = new() { Id = -1, Name = "Foo" } };

        var check = Context.Check(dto.Child).ValidateWith(Validator, shortCircuitOnError: true);

        Context.ShouldHaveErrors();
        check.ShouldBeShortCircuited();
    }

    [Fact]
    public void NoShortCircuitWithValidChild()
    {
        var dto = new ComplexDto { Child = new() { Id = 42, Name = "Jane Doe" } };

        var check = Context.Check(dto.Child).ValidateWith(Validator, shortCircuitOnError: true);

        Context.ShouldHaveNoErrors();
        check.ShouldNotBeShortCircuited();
    }
}

public sealed class ChildDtoValidator : Validator<ChildDto>
{
    public ChildDtoValidator(IValidationContextFactory validationContextFactory)
        : base(validationContextFactory) { }

    protected override ChildDto PerformValidation(ValidationContext context, ChildDto dto)
    {
        context.Check(dto.Id).IsGreaterThan(0);
        dto.Name = context.Check(dto.Name).HasLengthIn(Range.FromInclusive(2).ToInclusive(100));
        return dto;
    }
}