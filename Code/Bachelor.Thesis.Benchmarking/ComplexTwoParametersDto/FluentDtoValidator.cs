using FluentValidation;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class FluentDtoValidator : AbstractValidator<ComplexTwoParametersDto>
{
    public FluentDtoValidator()
    {
        RuleFor(dto => dto.User).SetValidator(UserValidator);
        RuleFor(dto => dto.Address).SetValidator(AddressValidator);
    }

    private FluentAddressValidator AddressValidator { get; } = new ();
    private FluentUserValidator UserValidator { get; } = new ();
}