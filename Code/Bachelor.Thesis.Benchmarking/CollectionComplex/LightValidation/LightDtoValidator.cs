using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.LightValidation;

public class LightDtoValidator : Validator<CollectionComplexDto>
{
    public LightDtoValidator() : base(Light.Validation.ValidationContextFactory.Instance) { }

    protected override CollectionComplexDto PerformValidation(ValidationContext context, CollectionComplexDto value)
    {
        context.Check(value.ArticleList.Count).IsIn(Range.FromInclusive(1).ToInclusive(10));
        context.Check(value.ArticleList)
               .ValidateItems((Check<Article> article) =>
                                  article.ValidateWith(new LightArticleValidator()));

        context.Check(value.OrderDetailsList.Count).IsIn(Range.FromInclusive(1).ToInclusive(10));
        context.Check(value.OrderDetailsList)
               .ValidateItems((Check<OrderDetails> orderDetails) =>
                                  orderDetails.ValidateWith(new LightOrderDetailsValidator()));

        return value;
    }
}