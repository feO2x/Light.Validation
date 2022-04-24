using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.PrimitiveParameters;

public class EmployeeDto
{
    public static EmployeeDto ValidEmployeeDto = new ()
    {
        Id = Guid.NewGuid(),
        Name = "John Doe",
        Position = 'B',
        Department = 420,
        WeeklyWorkingHours = 40,
        EmployeeId = 12345L,
        ProductivityScore = 82.3,
        OvertimeWorked = 143.423f,
        HourlySalary = new decimal(16.50)
    };

    public static EmployeeDto InvalidEmployeeDto = new ()
    {
        Id = new (),
        Name = "   x     ",
        Position = 'a',
        Department = 98,
        WeeklyWorkingHours = 50,
        EmployeeId = 1023L,
        ProductivityScore = 120.1,
        OvertimeWorked = -110.23f,
        HourlySalary = new decimal(8.50)
    };

    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range('A', 'Z')]
    public char Position { get; set; }

    [Required]
    [Range(100, 999)]
    public short Department { get; set; }

    [Required]
    [Range(20, 48)]
    public int WeeklyWorkingHours { get; set; }

    [Required]
    [Range(10000L, 99999L)]
    public long EmployeeId { get; set; }

    [Required]
    [Range(0.0, 100.0)]
    public double ProductivityScore { get; set; }

    [Required]
    [Range(-100.0, 200.0)]
    public float OvertimeWorked { get; set; }

    [Required]
    [Range(12.0, 999.9)]
    public decimal HourlySalary { get; set; }
}