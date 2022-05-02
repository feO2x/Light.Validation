using Light.Validation.Checks;

namespace Light.Validation.Benchmarks;

public sealed class LightValidator : Validator<UpdateUserNameDto>
{
    public LightValidator(IValidationContextFactory validationContextFactory) : base(validationContextFactory) { }

    protected override UpdateUserNameDto PerformValidation(ValidationContext context, UpdateUserNameDto dto)
    {
        context.Check(dto.Id).IsGreaterThan(0);
        dto.UserName = context.Check(dto.UserName)
                              .IsNotNullOrWhiteSpace();
        return dto;
    }
}