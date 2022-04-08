using FluentValidation;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.FluentValidator;

public class FluentDtoValidator : AbstractValidator<Dto.CustomerDto>
{
    public FluentDtoValidator()
    {
        RuleFor(dto => dto.User).SetValidator(UserValidator);
        RuleFor(dto => dto.Address).SetValidator(AddressValidator);
    }

    private FluentAddressValidator AddressValidator { get; } = new ();
    private FluentUserValidator UserValidator { get; } = new ();
}