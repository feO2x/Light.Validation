using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.FlatTwoParametersDto.Validators;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class ValidDtoBenchmarks
{
    public FluentValidator FluentValidator = new ();

    public LightValidator LightValidator = new ();

    [ParamsSource(nameof(ValuesForDto))]
    public FlatTwoParametersDto Dto { get; set; } = null!;

    public static IEnumerable<FlatTwoParametersDto> ValuesForDto => new[]
    {
        FlatTwoParametersDto.ValidDto,
        FlatTwoParametersDto.InvalidDto
    };

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