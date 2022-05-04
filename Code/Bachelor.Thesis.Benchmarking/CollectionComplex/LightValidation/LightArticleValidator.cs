using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.LightValidation;

public class LightArticleValidator : Validator<Article>
{
    public LightArticleValidator() : base(Light.Validation.ValidationContextFactory.Instance) { }

    protected override Article PerformValidation(ValidationContext context, Article value)
    {
        value.Id = context.Check(value.Id).IsIn(Range.FromInclusive((long) 10000).ToInclusive(long.MaxValue));
        value.Title = context.Check(value.Title).IsNotNullOrWhiteSpace().HasLengthIn(Range.FromInclusive(10).ToInclusive(100));
        value.Price = context.Check(value.Price).IsIn(Range.FromInclusive((decimal) 0.01).ToInclusive((decimal) 10000.0));
        value.Quantity = context.Check(value.Quantity).IsIn(Range.FromInclusive(1).ToInclusive(10000));

        return value;
    }
}