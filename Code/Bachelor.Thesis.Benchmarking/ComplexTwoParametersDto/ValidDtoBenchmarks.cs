﻿using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class ValidDtoBenchmarks
{
    public ComplexTwoParametersDto Dto = new () { User = User.ValidUser, Address = Address.ValidAddress };

    public FluentValidator FluentValidator = new ();
    public LightValidator LightValidator = new ();

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