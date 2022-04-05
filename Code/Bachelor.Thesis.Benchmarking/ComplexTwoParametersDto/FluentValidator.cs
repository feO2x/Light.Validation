using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class FluentValidator : AbstractValidator<ComplexTwoParametersDto>
{
    public FluentValidator()
    {
        RuleFor(dto => dto.User.UserName).NotEmpty().MinimumLength(8).MaximumLength(30);
        RuleFor(dto => dto.User.Password).NotEmpty().Matches(new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"));
        RuleFor(dto => dto.User.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(dto => dto.User.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.User.Age).InclusiveBetween(18, 130);
        
        RuleFor(dto => dto.Address.Country).NotEmpty().MinimumLength(1).MaximumLength(40);
        RuleFor(dto => dto.Address.Region).NotEmpty().MinimumLength(1).MaximumLength(40);
        RuleFor(dto => dto.Address.City).NotEmpty().MinimumLength(1).MaximumLength(40);
        RuleFor(dto => dto.Address.Street).NotEmpty().MinimumLength(1).MaximumLength(80);
        RuleFor(dto => dto.Address.PostalCode).InclusiveBetween(10000, 99999);
    }

    protected override bool PreValidate(ValidationContext<ComplexTwoParametersDto> context, ValidationResult result)
    {
        context.InstanceToValidate.User.UserName = context.InstanceToValidate.User.UserName.Trim();
        context.InstanceToValidate.User.Password = context.InstanceToValidate.User.Password.Trim();
        context.InstanceToValidate.User.Name = context.InstanceToValidate.User.Name.Trim();
        context.InstanceToValidate.User.Email = context.InstanceToValidate.User.Email.Trim();

        context.InstanceToValidate.Address.Country = context.InstanceToValidate.Address.Country.Trim();
        context.InstanceToValidate.Address.Region = context.InstanceToValidate.Address.Region.Trim();
        context.InstanceToValidate.Address.City = context.InstanceToValidate.Address.City.Trim();
        context.InstanceToValidate.Address.Street = context.InstanceToValidate.Address.Street.Trim();

        return true;
    }
}