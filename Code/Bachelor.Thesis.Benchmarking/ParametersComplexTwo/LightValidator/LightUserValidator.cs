using System.Text.RegularExpressions;
using Bachelor.Thesis.Benchmarking.ParametersComplexTwo.Dto;
using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ParametersComplexTwo.LightValidator;

public class LightUserValidator : Validator<User>
{
    public LightUserValidator() : base(Light.Validation.ValidationContextFactory.Instance) { }

    protected override User PerformValidation(ValidationContext context, User user)
    {
        user.UserName = context.Check(user.UserName)
                               .HasLengthIn(Range.FromInclusive(8).ToInclusive(30));

        user.Password = context.Check(user.Password)
                               .IsMatching(new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"));

        user.Name = context.Check(user.Name)
                           .HasLengthIn(Range.FromInclusive(2).ToInclusive(100));

        user.Email = context.Check(user.Email)
                            .IsEmail();

        user.Age = context.Check(user.Age)
                          .IsIn(Range.FromInclusive(18).ToInclusive(130));

        return user;
    }
}