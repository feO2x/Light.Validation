using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.FlatEightParameters;
using Bachelor.Thesis.Benchmarking.FlatEightParameters.Validators;
using Light.GuardClauses;
using Xunit;

namespace Bachelor.Thesis.Tests.Validators;

public class FlatEightParametersValidatorTest
{
    private readonly EmployeeDto _validEmployee = EmployeeDto.ValidEmployeeDto;
    private readonly EmployeeDto _invalidEmployee = EmployeeDto.InvalidEmployeeDto;

    [Fact]
    public void FluentValidatorValidDtoTest()
    {
        var fluentValidator = new FluentValidator();
        var result = fluentValidator.Validate(_validEmployee);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void LightValidatorValidDtoTest()
    {
        var lightValidator = new LightValidator();
        var result = lightValidator.Validate(_validEmployee);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void ModelValidatorValidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_validEmployee, new ValidationContext(_validEmployee), errors, true);

        result.MustBe(true);
    }

    [Fact]
    public void FluentValidatorInvalidDtoTest()
    {
        var fluentValidator = new FluentValidator();
        var result = fluentValidator.Validate(_invalidEmployee);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void LightValidatorInvalidDtoTest()
    {
        var lightValidator = new LightValidator();
        var result = lightValidator.Validate(_invalidEmployee);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void ModelValidatorInvalidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_invalidEmployee, new ValidationContext(_invalidEmployee), errors, true);

        result.MustBe(false);
    }
}