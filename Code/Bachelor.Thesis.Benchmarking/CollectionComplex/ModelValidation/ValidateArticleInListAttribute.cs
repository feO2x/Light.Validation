using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.ModelValidation;

public class ValidateArticleInListAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var articleList = value as List<Article>;

        if (articleList == null)
            return ValidationResult.Success;

        var error = new List<ValidationResult>();

        foreach (var article in articleList)
        {
            Validator.TryValidateObject(article, new ValidationContext(article), error);
            if (error.Count > 0)
                return new ValidationResult(error.ToString());
        }

        return ValidationResult.Success;
    }
}