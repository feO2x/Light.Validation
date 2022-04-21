using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;
using Bachelor.Thesis.Benchmarking.ComplexTwoParameters.FluentValidator;
using Bachelor.Thesis.Benchmarking.ComplexTwoParameters.LightValidator;
using Light.GuardClauses;
using Xunit;

namespace Bachelor.Thesis.Tests.Validators;

public class ComplexTwoParametersValidatorTest
{
    private readonly CustomerDto _validCustomer = CustomerDto.ValidCustomerDto;
    private readonly CustomerDto _invalidCustomer = CustomerDto.InvalidCustomerDto;

    [Fact]
    public void FluentValidatorValidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_validCustomer);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void LightValidatorValidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_validCustomer);

        result.IsValid.MustBe(true);
    }

    [Fact]
    public void ModelValidatorValidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_validCustomer, new ValidationContext(_validCustomer), errors, true);

        var errorsNestedObjectUser = new List<ValidationResult>();
        var resultNestedObjectUser = Validator.TryValidateObject(User.ValidUser, new ValidationContext(User.ValidUser), errorsNestedObjectUser, true);

        var errorsNestedObjectAddress = new List<ValidationResult>();
        var resultNestedObjectAddress = Validator.TryValidateObject(Address.ValidAddress, new ValidationContext(Address.ValidAddress), errorsNestedObjectAddress, true);

        result.MustBe(true);
        resultNestedObjectUser.MustBe(true);
        resultNestedObjectAddress.MustBe(true);
    }

    [Fact]
    public void FluentValidatorInvalidDtoTest()
    {
        var fluentValidator = new FluentDtoValidator();
        var result = fluentValidator.Validate(_invalidCustomer);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void LightValidatorInvalidDtoTest()
    {
        var lightValidator = new LightDtoValidator();
        var result = lightValidator.Validate(_invalidCustomer);

        result.IsValid.MustBe(false);
    }

    [Fact]
    public void ModelValidatorInvalidDtoTest()
    {
        var errors = new List<ValidationResult>();
        var result = Validator.TryValidateObject(_invalidCustomer, new ValidationContext(_invalidCustomer), errors, true);

        var errorsNestedObjectUser = new List<ValidationResult>();
        var resultNestedObjectUser = Validator.TryValidateObject(User.InvalidUser, new ValidationContext(User.InvalidUser), errorsNestedObjectUser, true);

        var errorsNestedObjectAddress = new List<ValidationResult>();
        var resultNestedObjectAddress = Validator.TryValidateObject(Address.InvalidAddress, new ValidationContext(Address.InvalidAddress), errorsNestedObjectAddress, true);

        result.MustBe(true);
        resultNestedObjectUser.MustBe(false);
        resultNestedObjectAddress.MustBe(false);
    }
}