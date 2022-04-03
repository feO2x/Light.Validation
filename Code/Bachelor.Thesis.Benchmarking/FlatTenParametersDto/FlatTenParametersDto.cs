using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.FlatTenParametersDto;

public class FlatTenParametersDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool Active { get; set; } = false;

    [Required]
    public short Role { get; set; }

    [Required]
    public int MonthlySalary { get; set; }

    [Required]
    public long PhoneNumber { get; set; }

    [Required]
    public char Department { get; set; }

    [Required]
    public float PerformanceRating { get; set; }

    [Required]
    public double HourlySalary { get; set; }
}