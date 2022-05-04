using Bachelor.Thesis.Benchmarking.ParametersComplexTwo.Dto;
using FluentValidation;

namespace Bachelor.Thesis.Benchmarking.ParametersComplexTwo.FluentValidator;

public class FluentDtoValidator : AbstractValidator<CustomerDto>
{
    public FluentDtoValidator()
    {
        RuleFor(dto => dto.User).SetValidator(UserValidator);
        RuleFor(dto => dto.Address).SetValidator(AddressValidator);
    }

    private FluentAddressValidator AddressValidator { get; } = new ();
    private FluentUserValidator UserValidator { get; } = new ();
}