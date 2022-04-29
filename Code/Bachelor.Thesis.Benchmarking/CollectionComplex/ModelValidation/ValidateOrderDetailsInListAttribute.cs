using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.ModelValidation;

public class ValidateOrderDetailsInListAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var orderDetailsList = value as List<OrderDetails>;

        if (orderDetailsList == null) 
            return ValidationResult.Success;

        var error = new List<ValidationResult>();

        foreach (var order in orderDetailsList)
        {
            Validator.TryValidateObject(order, new ValidationContext(order), error);
            if (error.Count > 0)
                return new ValidationResult(error.ToString());
        }

        return ValidationResult.Success;
    }
}