using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class ModelValidationAddressDto
{
    public static Address ValidAddress = new()
    {
        Country = "Germany",
        City = "Regensburg",
        Region = "Bavaria",
        PostalCode = 93053,
        Street = "Seybothstraße 2"
    };

    [Required]
    [MinLength(1)]
    public string City { get; set; } = String.Empty;

    [Required]
    [MinLength(1)]
    public string Country { get; set; } = String.Empty;

    [Required]
    [Range(10000, 99999)]
    public int PostalCode { get; set; }

    [Required]
    [MinLength(1)]
    public string Region { get; set; } = String.Empty;

    [Required]
    [MinLength(1)]
    public string Street { get; set; } = String.Empty;
}