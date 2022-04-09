using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.FlatCollection.Validators;

public class NoStringInCollectionBiggerThan : ValidationAttribute
{
    private readonly int _length;

    public NoStringInCollectionBiggerThan(int length) =>
        _length = length;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var strings = value as IEnumerable<string>;
        if (strings == null)
        {
            return ValidationResult.Success;
        }

        var invalid = strings.Where(s => s.Length > _length).ToArray();
        if (invalid.Length > 0)
        {
            return new ValidationResult("The following strings exceed the value: " + string.Join(", ", invalid));
        }

        return ValidationResult.Success;
    }
}