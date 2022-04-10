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


        return collectionDto;
    }
}