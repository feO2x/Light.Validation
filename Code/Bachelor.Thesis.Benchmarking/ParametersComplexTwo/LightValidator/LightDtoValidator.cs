using Bachelor.Thesis.Benchmarking.ParametersComplexTwo.Dto;
using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.ParametersComplexTwo.LightValidator;

public class LightDtoValidator : Validator<CustomerDto>
{
    public LightDtoValidator() : base(Light.Validation.ValidationContextFactory.Instance) { }

    private LightAddressValidator AddressValidator { get; } = new ();
    private LightUserValidator UserValidator { get; } = new ();

    protected override CustomerDto PerformValidation(ValidationContext context, CustomerDto dto)
    {
        dto.User = context.Check(dto.User).ValidateWith(UserValidator);

        dto.Address = context.Check(dto.Address).ValidateWith(AddressValidator);

        return dto;
    }
}