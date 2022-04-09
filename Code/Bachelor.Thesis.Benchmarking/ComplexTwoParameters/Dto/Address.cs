using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;

public class Address
{
    public static Address ValidAddress = new ()
    {
        Country = "Germany",
        City = "Regensburg",
        Region = "Bavaria",
        PostalCode = 93053,
        Street = "Seybothstraße 2"
    };

    public static Address InvalidAddress = new ()
    {
        Country = string.Empty,
        PostalCode = 1000,
        Region = string.Empty,
        Street = string.Empty
    };

    [Required]
    [MinLength(1)]
    [MaxLength(40)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    [MaxLength(40)]
    public string Country { get; set; } = string.Empty;

    [Required]
    [Range(10000, 99999)]
    public int PostalCode { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(40)]
    public string Region { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    [MaxLength(80)]
    public string Street { get; set; } = string.Empty;
}