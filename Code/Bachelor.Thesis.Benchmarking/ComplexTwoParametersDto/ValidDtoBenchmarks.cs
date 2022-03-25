using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class ValidDtoBenchmarks
{
    public ComplexTwoParametersDto Dto = new () { Names = new () { "John", "Doe" }, Address = Address.ValidAddress };

    public FluentValidator FluentValidator = new ();
    public LightValidator LightValidator = new ();

    public ModelValidationDto ModelValidationDto = new () { Names = new () { "John", "Doe" }, Address = ModelValidationAddress.ValidAddress };

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
        Validator.TryValidateObject(ModelValidationDto, new ValidationContext(ModelValidationDto), errors, true);
        return errors;
    }
}