using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class FlatTwoParametersDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(80)]
    public string Name { get; set; } = String.Empty;
}