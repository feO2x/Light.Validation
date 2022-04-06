using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.FlatEightParametersDto.Validators;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.FlatEightParametersDto;

public class InvalidDtoBenchmarks
{
    public Employee Employee = Employee.InvalidEmployee;

    public FluentValidator FluentValidator = new();

    public LightValidator LightValidator = new();

    [Benchmark(Baseline = true)]
    public object? CheckViaLightValidator()
    {
        LightValidator.CheckForErrors(Employee, out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaFluentValidator() =>
        FluentValidator.Validate(Employee);

    [Benchmark]
    public object CheckViaModelValidation()
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(Employee, new ValidationContext(Employee), errors, true);
        return errors;
    }
}