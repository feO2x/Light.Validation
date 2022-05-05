using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using Bachelor.Thesis.Benchmarking.CollectionComplex.FluentValidation;
using Bachelor.Thesis.Benchmarking.CollectionComplex.LightValidation;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex;

public class ValidCollectionComplexBenchmarks
{
    public CollectionComplexDto Dto = CollectionComplexDto.ValidDto;

    public FluentDtoValidator FluentValidator = new();
    public LightDtoValidator LightValidator = new();

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