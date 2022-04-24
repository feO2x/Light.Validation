using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionFlat.Validators;

public class NoLongInDictionaryBiggerThanAttribute : ValidationAttribute
{
    private readonly int _maxValue;

    public NoLongInDictionaryBiggerThanAttribute(int maxValue) =>
        _maxValue = maxValue;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dictionary = value as IDictionary<long, bool>;
        if (dictionary == null)
            return ValidationResult.Success;

        var invalid = dictionary.Where(l => l.Key > _maxValue).ToArray();
        if (invalid.Length > 0)
        {
            return new ValidationResult("The following keys exceed the value: " + string.Join(", ", invalid));
        }

        return ValidationResult.Success;
    }
}