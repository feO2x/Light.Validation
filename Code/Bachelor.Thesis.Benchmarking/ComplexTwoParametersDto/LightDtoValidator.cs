using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class LightDtoValidator : Validator<ComplexTwoParametersDto>
{
    private LightAddressValidator AddressValidator { get; } = new ();
    private LightUserValidator UserValidator { get; } = new ();

    protected override ComplexTwoParametersDto PerformValidation(ValidationContext context, ComplexTwoParametersDto dto)
    {
        dto.User = context.Check(dto.User).ValidateWith(UserValidator);

        dto.Address = context.Check(dto.Address).ValidateWith(AddressValidator);

        return dto;
    }
}