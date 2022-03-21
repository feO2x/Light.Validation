using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class FluentValidatorOfModelValidation : AbstractValidator<ModelValidationDto>
{
    public FluentValidatorOfModelValidation()
    {
        RuleFor(dto => dto.Id).GreaterThan(0);
        RuleFor(dto => dto.Name).NotEmpty();
    }

    protected override bool PreValidate(ValidationContext<ModelValidationDto> context, ValidationResult result)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier -- value can be null
        context.InstanceToValidate.Name = context.InstanceToValidate.Name?.Trim() ?? string.Empty;
        return true;
    }
}