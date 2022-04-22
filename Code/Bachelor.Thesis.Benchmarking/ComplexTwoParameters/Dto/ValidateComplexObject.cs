using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;

public class ValidateComplexObject : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("The object " + nameof(value) + " is null.");
        }

        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(value, new ValidationContext(value), errors);

        if (errors.Count > 0)
        {
            var validationResultString = string.Empty;

            foreach (var error in errors)
            {
                validationResultString += error.ErrorMessage + ", ";
            }
            
            return new ValidationResult("The following error messages have been thrown for object " + nameof(value) + ":" + validationResultString);
        }

        return ValidationResult.Success;
    }
}