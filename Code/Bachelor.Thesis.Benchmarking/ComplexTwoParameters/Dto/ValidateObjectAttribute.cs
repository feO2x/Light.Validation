using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;

public class ValidateObjectAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("The object " + nameof(value) + " is null.");
        }

        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(value, new ValidationContext(value), errors);

        if (errors.Count != 0)
        {
            var compositeResults = new CompositeValidationResult($"Validation for {validationContext.DisplayName} failed!");

            return compositeResults;
        }

        return ValidationResult.Success;
    }
}