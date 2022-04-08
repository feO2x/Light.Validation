using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.LightValidator;

public class LightDtoValidator : Validator<Dto.CustomerDto>
{
    private LightAddressValidator AddressValidator { get; } = new ();
    private LightUserValidator UserValidator { get; } = new ();

    protected override Dto.CustomerDto PerformValidation(ValidationContext context, Dto.CustomerDto dto)
    {
        dto.User = context.Check(dto.User).ValidateWith(UserValidator);

        dto.Address = context.Check(dto.Address).ValidateWith(AddressValidator);

        return dto;
    }
}