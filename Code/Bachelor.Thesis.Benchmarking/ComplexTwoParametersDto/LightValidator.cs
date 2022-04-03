using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class LightValidator : Validator<ComplexTwoParametersDto>
{
    protected override ComplexTwoParametersDto PerformValidation(ValidationContext context, ComplexTwoParametersDto value)
    {
        context.Check(value.Names.Count).IsGreaterThanOrEqualTo(2);

        value.Address.Country = context.Check(value.Address.Country)
                                         .Normalize()
                                         .IsNotNullOrWhiteSpace();

        value.Address.Region = context.Check(value.Address.Region)
                                        .Normalize()
                                        .IsNotNullOrWhiteSpace();

        value.Address.City = context.Check(value.Address.City)
                                      .Normalize()
                                      .IsNotNullOrWhiteSpace();

        value.Address.Street = context.Check(value.Address.Street)
                                        .Normalize()
                                        .IsNotNullOrWhiteSpace();

        value.Address.PostalCode = context.Check(value.Address.PostalCode).IsIn(Range.FromInclusive(10000).ToInclusive(99999));

        return value;
    }
}