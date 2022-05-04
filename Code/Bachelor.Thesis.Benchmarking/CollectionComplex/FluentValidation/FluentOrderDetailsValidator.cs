using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using FluentValidation;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.FluentValidation;

public class FluentOrderDetailsValidator : AbstractValidator<OrderDetails>
{
    public FluentOrderDetailsValidator()
    {
        RuleFor(orderDetails => orderDetails.OrderId).NotEmpty().InclusiveBetween(10000, long.MaxValue);
        RuleFor(orderDetails => orderDetails.ProductId).NotEmpty().InclusiveBetween(10000, long.MaxValue);
        RuleFor(orderDetails => orderDetails.Date).NotEmpty().InclusiveBetween(new DateTime(2020, 01, 01), new DateTime(2022, 12, 31));
        RuleFor(orderDetails => orderDetails.Quantity).NotEmpty().InclusiveBetween(0, 1000);
        RuleFor(orderDetails => orderDetails.PricePaid).NotEmpty().InclusiveBetween((decimal) 0.01, (decimal) 100000.0);
    }
}