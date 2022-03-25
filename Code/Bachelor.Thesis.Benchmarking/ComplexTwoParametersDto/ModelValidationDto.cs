using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class ModelValidationDto
{
    [Required]
    [MinLength(1)]
    public List<string> ListOfStrings { get; set; } = new();

    [Required]
    public ModelValidationAddress Address { get; set; } = new();
}