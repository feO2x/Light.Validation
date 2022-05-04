using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;
using Bachelor.Thesis.Benchmarking.CollectionComplex.FluentValidation;
using Bachelor.Thesis.Benchmarking.CollectionComplex.LightValidation;
using Light.GuardClauses;
using Xunit;
using Xunit.Abstractions;

namespace Bachelor.Thesis.Tests.Validators;

public class CollectionComplexValidatorTest
{
    public ITestOutputHelper Output { get; }

    public CollectionComplexValidatorTest(ITestOutputHelper output)
    {
        Output = output;
    }

    private readonly CollectionComplexDto _valid = CollectionComplexDto.ValidDto;
    private readonly CollectionComplexDto _invalid = CollectionComplexDto.InvalidDto;

    [Fact]
    public void FluentValidatorValidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_valid);

        Output.WriteLine(Json.Serialize(result));

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void LightValidatorValidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_valid);

        Output.WriteLine(Json.Serialize(result));

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void ModelValidatorValidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_valid, new ValidationContext(_valid), errors, true);

        Output.WriteLine(Json.Serialize(errors));

        result.MustBe(true);
    }

    [Fact]
    public void FluentValidatorInvalidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_invalid);

        Output.WriteLine(Json.Serialize(result));

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void LightValidatorInvalidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_invalid);

        Output.WriteLine(Json.Serialize(result));

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void ModelValidatorInvalidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_invalid, new ValidationContext(_invalid), errors, true);

        Output.WriteLine(Json.Serialize(errors));

        result.MustBe(false);
    }
}