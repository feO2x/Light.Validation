using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto.Validators;

public class FluentValidator : AbstractValidator<FlatTwoParametersDto>
{
    public FluentValidator()
    {
        RuleFor(dto => dto.Id).GreaterThan(0);
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(80);
    }

    protected override bool PreValidate(ValidationContext<FlatTwoParametersDto> context, ValidationResult result)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier -- value can be null
        context.InstanceToValidate.Name = context.InstanceToValidate.Name?.Trim() ?? string.Empty;
        return true;
    }
}