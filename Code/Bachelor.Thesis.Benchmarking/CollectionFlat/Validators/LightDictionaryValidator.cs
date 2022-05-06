using Light.Validation;
using Light.Validation.Checks;

namespace Bachelor.Thesis.Benchmarking.CollectionFlat.Validators;

public class LightDictionaryValidator : Validator<Dictionary<long, bool>>
{
    public LightDictionaryValidator() : base(Light.Validation.ValidationContextFactory.Instance) { }

    protected override Dictionary<long, bool> PerformValidation(ValidationContext context, Dictionary<long, bool> value)
    {
        context.Check(new List<long>(value.Keys))
               .ValidateItems((Check<long> key) =>
                                  key.IsLessThanOrEqualTo(10000));

        return value;
    }
}