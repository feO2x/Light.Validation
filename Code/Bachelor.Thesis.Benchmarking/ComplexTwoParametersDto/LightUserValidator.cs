using System.Text.RegularExpressions;
using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class LightUserValidator : Validator<User>
{
    protected override User PerformValidation(ValidationContext context, User user)
    {
        user.UserName = context.Check(user.UserName)
                               .IsLongerThan(8)
                               .IsShorterThan(30);

        user.Password = context.Check(user.Password)
                               .IsMatching(new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"));

        user.Name = context.Check(user.Name)
                           .IsLongerThan(2)
                           .IsShorterThan(100);

        user.Email = context.Check(user.Email)
                            .IsEmail();

        user.Age = context.Check(user.Age)
                          .IsIn(Range.FromInclusive(18).ToInclusive(130));

        return user;
    }
}