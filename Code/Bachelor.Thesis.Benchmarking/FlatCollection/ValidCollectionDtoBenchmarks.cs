using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.FlatCollection.Validators;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.FlatCollection;

public class ValidCollectionDtoBenchmarks
{
    public FlatCollection Dto = FlatCollection.ValidDto;

    public FluentDtoValidator FluentValidator = new ();
    public LightDtoValidator LightValidator = new ();

    [Benchmark(Baseline = true)]
    public object? CheckViaLightValidator()
    {
        LightValidator.CheckForErrors(Dto, out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaFluentValidator() =>
        FluentValidator.Validate(Dto);

    [Benchmark]
    public object CheckViaModelValidation()
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(Dto, new ValidationContext(Dto), errors, true);
        return errors;
    }
}