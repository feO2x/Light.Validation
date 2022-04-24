using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.CollectionFlat;
using Bachelor.Thesis.Benchmarking.CollectionFlat.Validators;
using Light.GuardClauses;
using Xunit;

namespace Bachelor.Thesis.Tests.Validators;

public class CollectionFlatValidatorTest
{
    private readonly FlatCollection _validCollection = FlatCollection.ValidDto;
    private readonly FlatCollection _invalidCollection = FlatCollection.InvalidDto;

    [Fact]
    public void FluentValidatorValidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_validCollection);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void LightValidatorValidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_validCollection);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void ModelValidatorValidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_validCollection, new ValidationContext(_validCollection), errors, true);

        result.MustBe(true);
    }

    [Fact]
    public void FluentValidatorInvalidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_invalidCollection);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void LightValidatorInvalidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_invalidCollection);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void ModelValidatorInvalidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_invalidCollection, new ValidationContext(_invalidCollection), errors, true);

        result.MustBe(false);
    }
}