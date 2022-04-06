using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Bachelor.Thesis.Benchmarking.FlatTenParametersDto;

public class Employee
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(100, 999)]
    public short Department { get; set; }

    [Required]
    [Range(20, 48)]
    public int WeeklyWorkingHours { get; set; }

    [Required]
    [Range(0, ulong.MaxValue)]
    public ulong PhoneNumber { get; set; }

    [Required]
    [Range(float.MinValue, float.MaxValue)]
    public float OvertimeWorked { get; set; }

    [Required]
    [Range(12.0, 999.9)]
    public decimal HourlySalary { get; set; }
}