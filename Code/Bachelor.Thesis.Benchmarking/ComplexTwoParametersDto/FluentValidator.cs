using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class FluentValidator : AbstractValidator<ComplexTwoParametersDto>
{
    public FluentValidator()
    {
        RuleFor(dto => dto.Names.Count).GreaterThanOrEqualTo(1);

        RuleFor(dto => dto.Address.Country).NotEmpty();
        RuleFor(dto => dto.Address.Region).NotEmpty();
        RuleFor(dto => dto.Address.City).NotEmpty();
        RuleFor(dto => dto.Address.Street).NotEmpty();
        RuleFor(dto => dto.Address.PostalCode).InclusiveBetween(10000, 99999);
    }

    [SuppressMessage("ReSharper", "ConstantConditionalAccessQualifier")]
    protected override bool PreValidate(ValidationContext<ComplexTwoParametersDto> context, ValidationResult result)
    {
        context.InstanceToValidate.Address.Country = context.InstanceToValidate.Address.Country?.Trim() ?? String.Empty;
        context.InstanceToValidate.Address.Region = context.InstanceToValidate.Address.Region?.Trim() ?? String.Empty;
        context.InstanceToValidate.Address.City = context.InstanceToValidate.Address.City?.Trim() ?? String.Empty;
        context.InstanceToValidate.Address.Street = context.InstanceToValidate.Address.Street?.Trim() ?? String.Empty;

        return true;
    }
}