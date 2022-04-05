using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto.Validators;

public class FluentValidator : AbstractValidator<FlatTwoParametersDto>
{
    public FluentValidator()
    {
        RuleFor(dto => dto.Id).GreaterThan(0).LessThan(int.MaxValue);
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(80);
    }

    protected override bool PreValidate(ValidationContext<FlatTwoParametersDto> context, ValidationResult result)
    {
        context.InstanceToValidate.Name = context.InstanceToValidate.Name.Trim();
        return true;
    }
}