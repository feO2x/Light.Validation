using Light.Validation;
using Light.Validation.Checks;
using Light.Validation.Tools;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class LightValidator : Validator<ComplexTwoParametersDto>
{
    protected override void PerformValidation(ValidationContext context, ComplexTwoParametersDto value)
    {
        context.Check(value.Names!.Count).IsGreaterThanOrEqualTo(1);

        value.Address.Country = context.Check(value.Address.Country)
                                       .Normalize()
                                       .IsNotNullOrWhiteSpace()
                                       .Value;

        value.Address.Region = context.Check(value.Address.Region)
                                       .Normalize()
                                       .IsNotNullOrWhiteSpace()
                                       .Value;

        value.Address.City = context.Check(value.Address.City)
                                       .Normalize()
                                       .IsNotNullOrWhiteSpace()
                                       .Value;

        value.Address.Street = context.Check(value.Address.Street)
                                       .Normalize()
                                       .IsNotNullOrWhiteSpace()
                                       .Value;

        context.Check(value.Address.PostalCode).IsIn(Range.FromInclusive(10000).ToInclusive(99999));
    }
}