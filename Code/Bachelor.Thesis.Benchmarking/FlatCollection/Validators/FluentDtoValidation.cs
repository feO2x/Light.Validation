using FluentValidation;

namespace Bachelor.Thesis.Benchmarking.FlatCollection.Validators;

public class FluentDtoValidation : AbstractValidator<FlatCollection>
{
    public FluentDtoValidation()
    {
        RuleFor(dto => dto.Names).NotEmpty();
        RuleFor(dto => dto.Names.Count).InclusiveBetween(1, 10);
        RuleForEach(dto => dto.Names).MaximumLength(20);

        RuleFor(dto => dto.Availability).NotEmpty();
        RuleForEach(dto => dto.Availability.Keys).LessThanOrEqualTo(10000);
    }
}