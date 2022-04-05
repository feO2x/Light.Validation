using System.Text.RegularExpressions;
using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class LightDtoValidator : Validator<ComplexTwoParametersDto>
{
    protected override ComplexTwoParametersDto PerformValidation(ValidationContext context, ComplexTwoParametersDto value)
    {
        // TODO: check User and Address in own validator classes
        value.User.UserName = context.Check(value.User.UserName)
                                       .IsLongerThan(8)
                                       .IsShorterThan(30);
        value.User.Password = context.Check(value.User.Password)
                                       .IsMatching(new Regex("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"));
        value.User.Name = context.Check(value.User.Name)
                                   .IsLongerThan(2)
                                   .IsShorterThan(100);
        value.User.Email = context.Check(value.User.Email)
                                    .IsEmail();
        value.User.Age = context.Check(value.User.Age)
                                  .IsIn(Range.FromInclusive(18).ToInclusive(130));

        value.Address.Country = context.Check(value.Address.Country)
                                         .IsNotNullOrWhiteSpace();
        value.Address.Region = context.Check(value.Address.Region)
                                        .IsNotNullOrWhiteSpace();
        value.Address.City = context.Check(value.Address.City)
                                      .IsNotNullOrWhiteSpace();
        value.Address.Street = context.Check(value.Address.Street)
                                        .IsNotNullOrWhiteSpace();
        value.Address.PostalCode = context.Check(value.Address.PostalCode)
                                            .IsIn(Range.FromInclusive(10000).ToInclusive(99999));

        return value;
    }
}