using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class ComplexTwoParametersDto
{
    [Required]
    [MinLength(2)]
    public List<string> Names { get; set; } = new();

    [Required]
    public Address Address { get; set; } = new();
}