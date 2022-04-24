using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.PrimitiveParameters.Validators;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.PrimitiveParameters;

public class InvalidEmployeeDtoBenchmarks
{
    public EmployeeDto EmployeeDto = EmployeeDto.InvalidEmployeeDto;

    public FluentValidator FluentValidator = new();

    public LightValidator LightValidator = new();

    [Benchmark(Baseline = true)]
    public object? CheckViaLightValidator()
    {
        LightValidator.CheckForErrors(EmployeeDto, out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaFluentValidator() =>
        FluentValidator.Validate(EmployeeDto);

    [Benchmark]
    public object CheckViaModelValidation()
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(EmployeeDto, new ValidationContext(EmployeeDto), errors, true);
        return errors;
    }
}