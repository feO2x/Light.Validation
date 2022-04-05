using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.LightValidator;

public class LightDtoValidator : Validator<Dto.ComplexTwoParametersDto>
{
    private LightAddressValidator AddressValidator { get; } = new ();
    private LightUserValidator UserValidator { get; } = new ();

    protected override Dto.ComplexTwoParametersDto PerformValidation(ValidationContext context, Dto.ComplexTwoParametersDto dto)
    {
        dto.User = context.Check(dto.User).ValidateWith(UserValidator);

        dto.Address = context.Check(dto.Address).ValidateWith(AddressValidator);

        return dto;
    }
}