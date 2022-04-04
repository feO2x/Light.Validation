using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class LightValidator : Validator<FlatTwoParametersDto>
{
    protected override FlatTwoParametersDto PerformValidation(ValidationContext context, FlatTwoParametersDto value)
    {
        value.Id = context.Check(value.Id).IsGreaterThan(0);

        value.Name = context.Check(value.Name)
                              .IsNotNullOrWhiteSpace()
                              .IsShorterThan(80);

        return value;
    }
}