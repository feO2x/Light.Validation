using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.CollectionFlat.Validators;

public class LightDtoValidator : Validator<CollectionFlatDto>
{
    public LightDtoValidator() : base(Light.Validation.ValidationContextFactory.Instance) { }

    protected override CollectionFlatDto PerformValidation(ValidationContext context, CollectionFlatDto dto)
    {
        dto.Names = context.Check(dto.Names).IsNotNull();
        context.Check(dto.Names.Count).IsIn(Range.FromInclusive(1).ToInclusive(10));

        context.Check(dto.Names)
               .ValidateItems((Check<string> name) =>
                                  name.IsShorterThan(20));

        dto.Availability = context.Check(dto.Availability).IsNotNull();

        // TODO: change to implicit call when dictionary is supported in light.Validation
        context.Check(new List<long>(dto.Availability.Keys))
               .ValidateItems((Check<long> key) =>
                                  key.IsLessThanOrEqualTo(10000));

        return dto;
    }
}