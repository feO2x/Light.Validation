﻿using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.ParametersPrimitiveTwo.Validators;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Bachelor.Thesis.Benchmarking.ParametersPrimitiveTwo;

public class ValidUserDtoBenchmarks
{
    public FluentValidator FluentValidator = new ();

    public LightValidator LightValidator = new ();

    public UserDto Dto = UserDto.ValidDto;

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