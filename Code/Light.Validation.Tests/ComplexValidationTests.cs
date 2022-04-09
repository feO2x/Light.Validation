using System;
using FluentAssertions;
using Light.Validation.Checks;
using Xunit;
using Xunit.Abstractions;
using Range = Light.Validation.Tools.Range;

namespace Light.Validation.Tests;

public sealed class ComplexValidationTests
{
    public ComplexValidationTests(ITestOutputHelper output) => Output = output;

    private ITestOutputHelper Output { get; }
    private ContactDtoValidator Validator { get; } = new (new ());

    [Theory]
    [MemberData(nameof(InvalidDtos))]
    public void ValidateWithInvalidDto(ContactDto invalidDto)
    {
        var result = Validator.Validate(invalidDto);

        Output.WriteLine(Json.Serialize(result.Errors));
        result.HasErrors.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidDtos))]
    public void CheckForErrorsWithInvalidDto(ContactDto invalidDto)
    {
        var result = Validator.CheckForErrors(invalidDto, out var errors);

        Output.WriteLine(Json.Serialize(errors));
        result.Should().BeTrue();
    }

    public static readonly TheoryData<ContactDto> InvalidDtos =
        new ()
        {
            new () { Name = "\t", DateOfBirth = new DateTime(1857, 3, 30) },
            new () { Name = "Valid Name", DateOfBirth = new DateTime(2000, 5, 13), Address = new () { Location = "", Street = "\t\r", ZipCode = "Not a ZIP code" } },
            new () { Name = "This name is OK", DateOfBirth = new DateTime(1879, 11, 14), Address = new () { Location = "Regensburg", ZipCode = "93049", Street = "University Street" } }
        };

    [Fact]
    public void ValidDto()
    {
        var validDto = new ContactDto
        {
            Name = "Foo",
            DateOfBirth = new DateTime(1978, 2, 18),
            Address = new ()
            {
                Location = "Regensburg",
                ZipCode = "93049",
                Street = "University Street"
            }
        };

        var result = Validator.Validate(validDto);

        result.IsValid.Should().BeTrue();
    }
}

public sealed class ContactDto
{
    public string Name { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public AddressDto Address { get; set; } = null!;
}

public sealed class AddressDto
{
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string Location { get; set; } = null!;
}

public sealed class ContactDtoValidator : Validator<ContactDto>
{
    public ContactDtoValidator(AddressDtoValidator addressValidator) => AddressValidator = addressValidator;

    private AddressDtoValidator AddressValidator { get; }

    protected override ContactDto PerformValidation(ValidationContext context, ContactDto dto)
    {
        dto.Name = context.Check(dto.Name).IsNotNullOrWhiteSpace();
        context.Check(dto.DateOfBirth).IsGreaterThan(new DateTime(1900, 1, 1));
        dto.Address = context.Check(dto.Address).ValidateWith(AddressValidator);
        return dto;
    }
}

public sealed class AddressDtoValidator : Validator<AddressDto>
{
    protected override AddressDto PerformValidation(ValidationContext context, AddressDto dto)
    {
        dto.Street = context.Check(dto.Street).IsNotNullOrWhiteSpace();
        dto.ZipCode = context.Check(dto.ZipCode)
                             .HasLengthIn(Range.FromInclusive(4).ToInclusive(5))
                             .ContainsOnlyDigits();
        dto.Location = context.Check(dto.Location).IsNotNullOrWhiteSpace();
        return dto;
    }
}