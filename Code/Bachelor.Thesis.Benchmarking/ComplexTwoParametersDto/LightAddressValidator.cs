using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class LightAddressValidator : Validator<Address>
{
    protected override Address PerformValidation(ValidationContext context, Address address)
    {
        address.City = context.Check(address.City)
                              .IsLongerThan(1)
                              .IsShorterThan(40);

        address.Country = context.Check(address.Country)
                                 .IsLongerThan(1)
                                 .IsShorterThan(40);

        address.PostalCode = context.Check(address.PostalCode)
                                    .IsIn(Range.FromInclusive(10000).ToInclusive(99999));

        address.Region = context.Check(address.Region)
                                .IsLongerThan(1)
                                .IsShorterThan(30);

        address.Street = context.Check(address.Street)
                                .IsLongerThan(1)
                                .IsShorterThan(50);

        return address;
    }
}