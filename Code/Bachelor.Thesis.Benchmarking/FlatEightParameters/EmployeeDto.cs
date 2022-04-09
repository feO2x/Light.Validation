using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.FlatEightParameters;

public class EmployeeDto
{
    public static EmployeeDto ValidEmployeeDto = new ()
    {
        Id = new (),
        Name = "John Doe",
        Department = 420,
        WeeklyWorkingHours = 40,
        PhoneNumber = "0123459876",
        OvertimeWorked = 143.423f,
        HourlySalary = new decimal(16.50)
    };

    public static EmployeeDto InvalidEmployeeDto = new ()
    {
        Id = new (),
        Name = "   x     ",
        Department = 98,
        WeeklyWorkingHours = 50,
        HourlySalary = new decimal(8.50)
    };

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
    [RegularExpression(@"^[0-9]")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [Range(float.MinValue, float.MaxValue)]
    public float OvertimeWorked { get; set; }

    [Required]
    [Range(12.0, 999.9)]
    public decimal HourlySalary { get; set; }
}