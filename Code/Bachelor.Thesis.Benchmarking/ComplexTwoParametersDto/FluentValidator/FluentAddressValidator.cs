using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.Dto;
using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.FluentValidator;

public class FluentAddressValidator : AbstractValidator<Address>
{
    public FluentAddressValidator()
    {
        RuleFor(address => address.Country).NotEmpty().MaximumLength(40);
        RuleFor(address => address.Region).NotEmpty().MaximumLength(40);
        RuleFor(address => address.City).NotEmpty().MaximumLength(40);
        RuleFor(address => address.Street).NotEmpty().MaximumLength(80);
        RuleFor(address => address.PostalCode).InclusiveBetween(10000, 99999);
    }

    protected override bool PreValidate(ValidationContext<Address> context, ValidationResult result)
    {
        context.InstanceToValidate.Country = context.InstanceToValidate.Country.Trim();
        context.InstanceToValidate.Region = context.InstanceToValidate.Region.Trim();
        context.InstanceToValidate.City = context.InstanceToValidate.City.Trim();
        context.InstanceToValidate.Street = context.InstanceToValidate.Street.Trim();

        return true;
    }
}