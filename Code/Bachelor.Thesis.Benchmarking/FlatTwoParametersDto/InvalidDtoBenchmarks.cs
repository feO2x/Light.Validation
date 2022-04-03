﻿using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class InvalidDtoBenchmarks
{
    public FlatTwoParametersDto Dto = new() { Id = -1, Name = String.Empty };

    public FluentValidator FluentValidator = new();
    public LightValidator LightValidator = new();

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