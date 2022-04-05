using System.Text.RegularExpressions;
using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.Dto;
using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.FluentValidator;

public class FluentUserValidator : AbstractValidator<User>
{
    public FluentUserValidator()
    {
        RuleFor(user => user.UserName).NotEmpty().MinimumLength(8).MaximumLength(30);
        RuleFor(user => user.Password).NotEmpty().Matches(new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"));
        RuleFor(user => user.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(user => user.Email).NotEmpty().EmailAddress();
        RuleFor(user => user.Age).InclusiveBetween(18, 130);
    }

    protected override bool PreValidate(ValidationContext<User> context, ValidationResult result)
    {
        context.InstanceToValidate.UserName = context.InstanceToValidate.UserName.Trim();
        context.InstanceToValidate.Password = context.InstanceToValidate.Password.Trim();
        context.InstanceToValidate.Name = context.InstanceToValidate.Name.Trim();
        context.InstanceToValidate.Email = context.InstanceToValidate.Email.Trim();

        return true;
    }
}