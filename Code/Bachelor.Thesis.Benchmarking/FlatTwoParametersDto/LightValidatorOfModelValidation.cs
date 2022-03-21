using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class LightValidatorOfModelValidation : Validator<ModelValidationDto>
{
    protected override void PerformValidation(ValidationContext context, ModelValidationDto value)
    {
        context.Check(value.Id).IsGreaterThan(0);
        value.Name = context.Check(value.Name)
                            .Normalize()
                            .IsNotNullOrWhiteSpace()
                            .Value;
    }
}