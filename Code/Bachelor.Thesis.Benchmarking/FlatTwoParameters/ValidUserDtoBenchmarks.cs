using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.FlatTwoParameters.Validators;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParameters;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByParams)]
public class ValidUserDtoBenchmarks
{
    public FluentValidator FluentValidator = new ();

    public LightValidator LightValidator = new ();

    [ParamsSource(nameof(ValuesForDto))]
    public UserDto Dto { get; set; } = null!;

    public static IEnumerable<UserDto> ValuesForDto => new[]
    {
        UserDto.ValidDto,
        UserDto.InvalidDto
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