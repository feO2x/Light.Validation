using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.FlatTwoParameters;
using Bachelor.Thesis.Benchmarking.FlatTwoParameters.Validators;
using Light.GuardClauses;
using Xunit;

namespace Bachelor.Thesis.Test;

public class FlatTwoParametersValidatorTest
{
    private readonly UserDto _validUser = UserDto.ValidDto;
    private readonly UserDto _invalidUser = UserDto.InvalidDto;

    [Fact]
    public void FluentValidatorValidDtoTest()
    {
        var fluentValidator = new FluentValidator();
        var result = fluentValidator.Validate(_validUser);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void LightValidatorValidDtoTest()
    {
        var lightValidator = new LightValidator();
        var result = lightValidator.Validate(_validUser);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void ModelValidatorValidDtoTest()
    {
        var result = Validator.TryValidateObject(_validUser, new ValidationContext(_validUser), null, true);

        result.MustBe(true);
    }

    [Fact]
    public void FluentValidatorInvalidDtoTest()
    {
        var fluentValidator = new FluentValidator();
        var result = fluentValidator.Validate(_invalidUser);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void LightValidatorInvalidDtoTest()
    {
        var lightValidator = new LightValidator();
        var result = lightValidator.Validate(_invalidUser);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void ModelValidatorInvalidDtoTest()
    {
        var result = Validator.TryValidateObject(_invalidUser, new ValidationContext(_invalidUser), null, true);

        result.MustBe(false);
    }
}