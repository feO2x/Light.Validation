using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.Dto;
using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.FluentValidator;
using Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto.LightValidator;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class InvalidDtoBenchmarks
{
    public Dto.ComplexTwoParametersDto Dto = new () { User = User.InvalidUser, Address = Address.InvalidAddress };

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