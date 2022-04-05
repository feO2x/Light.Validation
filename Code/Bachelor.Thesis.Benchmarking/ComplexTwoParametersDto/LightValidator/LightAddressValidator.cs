using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.Dto;
using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.LightValidator;

public class LightAddressValidator : Validator<Address>
{
    protected override Address PerformValidation(ValidationContext context, Address address)
    {
        address.City = context.Check(address.City)
                              .IsNotNullOrWhiteSpace()
                              .IsShorterThan(40);

        address.Country = context.Check(address.Country)
                                 .IsNotNullOrWhiteSpace()
                                 .IsShorterThan(40);

        address.PostalCode = context.Check(address.PostalCode)
                                    .IsIn(Range.FromInclusive(10000).ToInclusive(99999));

        address.Region = context.Check(address.Region)
                                .IsNotNullOrWhiteSpace()
                                .IsShorterThan(40);

        address.Street = context.Check(address.Street)
                                .IsNotNullOrWhiteSpace()
                                .IsShorterThan(80);

        return address;
    }
}