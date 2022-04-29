using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.ModelValidation;

public class ValidateOrderDetailsInListAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var collection = value as ICollection;

        return base.IsValid(value, validationContext);
    }
}