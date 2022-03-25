using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class ModelValidationDto
{
    [Required]
    [MinLength(2)]
    public List<string> Names { get; set; } = new();

    [Required]
    public ModelValidationAddress Address { get; set; } = new();
}