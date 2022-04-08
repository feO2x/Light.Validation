using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.Dto;
using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.FluentValidator;
using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.LightValidator;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

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