using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;
using Bachelor.Thesis.Benchmarking.ComplexTwoParameters.FluentValidator;
using Bachelor.Thesis.Benchmarking.ComplexTwoParameters.LightValidator;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParameters;

public class ValidCustomerDtoBenchmarks
{
    public Dto.CustomerDto Dto = new () { User = User.ValidUser, Address = Address.ValidAddress };

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