using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class ModelValidationDto
{
    [Required]
    [Range(0, Int32.MaxValue)]
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = String.Empty;
}