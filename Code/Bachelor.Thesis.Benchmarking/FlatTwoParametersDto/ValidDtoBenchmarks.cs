using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class ValidDtoBenchmarks
{
    public FlatTwoParametersDto Dto = new () { Id = 42, Name = "John Doe" };

    public FluentValidator FluentValidator = new ();
    public FluentValidatorOfModelValidation FluentValidatorOfModelValidation = new ();

    public LightValidator LightValidator = new ();
    public LightValidatorOfModelValidation LightValidatorOfModelValidation = new ();

    public ModelValidationDto ModelValidationDto = new () { Id = 42, Name = "John Doe" };

    [Benchmark(Baseline = true)]
    public object? CheckViaLightValidator()
    {
        LightValidator.CheckForErrors(Dto, out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckModelValidationObjectViaLightValidator()
    {
        LightValidatorOfModelValidation.CheckForErrors(ModelValidationDto, out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaFluentValidator() =>
        FluentValidator.Validate(Dto);

    [Benchmark]
    public object? CheckModelValidationObjectViaFluentValidator() =>
        FluentValidatorOfModelValidation.Validate(ModelValidationDto);

    [Benchmark]
    public object CheckViaModelValidation()
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(ModelValidationDto, new ValidationContext(ModelValidationDto), errors, true);
        return errors;
    }
}