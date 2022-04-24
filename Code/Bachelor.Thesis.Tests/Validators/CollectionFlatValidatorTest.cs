using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.CollectionFlat;
using Bachelor.Thesis.Benchmarking.CollectionFlat.Validators;
using Light.GuardClauses;
using Xunit;

namespace Bachelor.Thesis.Tests.Validators;

public class CollectionFlatValidatorTest
{
    private readonly CollectionFlatDto _valid = CollectionFlatDto.ValidDto;
    private readonly CollectionFlatDto _invalid = CollectionFlatDto.InvalidDto;

    [Fact]
    public void FluentValidatorValidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_valid);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void LightValidatorValidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_valid);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void ModelValidatorValidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_valid, new ValidationContext(_valid), errors, true);

        result.MustBe(true);
    }

    [Fact]
    public void FluentValidatorInvalidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_invalid);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void LightValidatorInvalidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_invalid);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void ModelValidatorInvalidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_invalid, new ValidationContext(_invalid), errors, true);

        result.MustBe(false);
    }
}