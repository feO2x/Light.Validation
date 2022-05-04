using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using FluentValidation;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.FluentValidation;

public class FluentDtoValidator : AbstractValidator<CollectionComplexDto>
{
    public FluentDtoValidator()
    {
        RuleFor(dto => dto.ArticleList.Count).InclusiveBetween(1, 10);
        RuleForEach(dto => dto.ArticleList).SetValidator(new FluentArticleValidator());

        RuleFor(dto => dto.OrderDetailsList.Count).InclusiveBetween(1, 10);
        RuleForEach(dto => dto.OrderDetailsList).SetValidator(new FluentOrderDetailsValidator());
    }
}