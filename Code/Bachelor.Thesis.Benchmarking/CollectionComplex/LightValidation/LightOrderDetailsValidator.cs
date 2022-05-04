using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.LightValidation;

public class LightOrderDetailsValidator : Validator<OrderDetails>
{
    public LightOrderDetailsValidator() : base(Light.Validation.ValidationContextFactory.Instance) { }

    protected override OrderDetails PerformValidation(ValidationContext context, OrderDetails value)
    {
        value.OrderId = context.Check(value.OrderId).IsIn(Range.FromInclusive((long) 10000).ToInclusive(long.MaxValue));
        value.ProductId = context.Check(value.ProductId).IsIn(Range.FromInclusive((long) 10000).ToInclusive(long.MaxValue));
        value.Date = context.Check(value.Date).IsIn(Range.FromInclusive(new DateTime(2020, 01, 01)).ToInclusive(new DateTime(2022, 12, 31)));
        value.Quantity = context.Check(value.Quantity).IsIn(Range.FromInclusive(0).ToInclusive(1000));
        value.PricePaid = context.Check(value.PricePaid).IsIn(Range.FromInclusive((decimal) 0.01).ToInclusive((decimal) 100000.0));

        return value;
    }
}