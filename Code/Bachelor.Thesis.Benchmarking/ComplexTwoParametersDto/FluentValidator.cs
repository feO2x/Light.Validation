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
        RuleFor(dto => dto.User.ForeName).NotEmpty().MinimumLength(2).MaximumLength(80);
        RuleFor(dto => dto.User.LastName).NotEmpty().MinimumLength(2).MaximumLength(80);
        RuleFor(dto => dto.User.Age).InclusiveBetween(18, 130);

        // TODO: add additional rules
        RuleFor(dto => dto.Address.Country).NotEmpty();
        RuleFor(dto => dto.Address.Region).NotEmpty();
        RuleFor(dto => dto.Address.City).NotEmpty();
        RuleFor(dto => dto.Address.Street).NotEmpty();
        RuleFor(dto => dto.Address.PostalCode).InclusiveBetween(10000, 99999);
    }

    protected override bool PreValidate(ValidationContext<ComplexTwoParametersDto> context, ValidationResult result)
    {
        context.InstanceToValidate.User.UserName = context.InstanceToValidate.User.UserName.Trim();
        context.InstanceToValidate.User.Password = context.InstanceToValidate.User.Password.Trim();
        context.InstanceToValidate.User.ForeName = context.InstanceToValidate.User.ForeName.Trim();
        context.InstanceToValidate.User.LastName = context.InstanceToValidate.User.LastName.Trim();

        context.InstanceToValidate.Address.Country = context.InstanceToValidate.Address.Country.Trim();
        context.InstanceToValidate.Address.Region = context.InstanceToValidate.Address.Region.Trim();
        context.InstanceToValidate.Address.City = context.InstanceToValidate.Address.City.Trim();
        context.InstanceToValidate.Address.Street = context.InstanceToValidate.Address.Street.Trim();

        return true;
    }
}