using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto.Validators;

public class LightValidator : Validator<UserDto>
{
    protected override UserDto PerformValidation(ValidationContext context, UserDto value)
    {
        value.Id = context.Check(value.Id).IsGreaterThan(0);

        value.Name = context.Check(value.Name)
                              .IsNotNullOrWhiteSpace()
                              .IsShorterThan(80);

        return value;
    }
}