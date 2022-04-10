using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.FlatCollection.Validators;

public class LightDtoValidator : Validator<FlatCollection>
{
    protected override FlatCollection PerformValidation(ValidationContext context, FlatCollection collectionDto)
    {
        collectionDto.Names = context.Check(collectionDto.Names).IsNotNull();
        context.Check(collectionDto.Names.Count).IsIn(Range.FromInclusive(1).ToInclusive(10));

        foreach (var name in collectionDto.Names)
        {
            context.Check(name).IsShorterThan(20);
        }

        collectionDto.Availability = context.Check(collectionDto.Availability).IsNotNull();

        foreach (var key in collectionDto.Availability.Keys)
        {
            context.Check(key).IsLessThanOrEqualTo(10000);
        }

        return collectionDto;
    }
}