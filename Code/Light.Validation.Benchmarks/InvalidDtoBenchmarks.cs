using BenchmarkDotNet.Attributes;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Light.Validation.Benchmarks;

public class InvalidDtoBenchmarks
{
    public UpdateUserNameDto Dto { get; } = new () { Id = 0, UserName = " " };

    public LightValidator LightValidator { get; } = new ();

    public FluentValidator FluentValidator { get; } = new ();

    [Benchmark(Baseline = true)]
    public object? CheckLightDtoDirectly()
    {
        Dto.CheckForErrors(out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaLightValidator()
    {
        LightValidator.CheckForErrors(Dto, out var errors);
        return errors;
    }

    [Benchmark]
    public object? CheckViaFluentValidator() =>
        FluentValidator.Validate(Dto);

    [Benchmark]
    public object? CheckViaFluentValidatorAndConvertToModelState()
    {
        var validationResult = FluentValidator.Validate(Dto);
        var modelState = new ModelStateDictionary();
        validationResult.AddToModelState(modelState, string.Empty);
        return modelState;
    }
}