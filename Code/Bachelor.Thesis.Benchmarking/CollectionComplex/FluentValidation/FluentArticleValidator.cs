using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using FluentValidation;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.FluentValidation;

public class FluentArticleValidator : AbstractValidator<Article>
{
    public FluentArticleValidator()
    {
        RuleFor(article => article.Id).NotEmpty().InclusiveBetween(10000, long.MaxValue);
        RuleFor(article => article.Title).NotEmpty().Length(10, 100);
        RuleFor(article => article.Price).NotEmpty().InclusiveBetween((decimal) 0.01, (decimal) 10000.0);
        RuleFor(article => article.Quantity).NotEmpty().InclusiveBetween(1, 10000);
    }
}