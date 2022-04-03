using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class User
{
    public static User ValidUser = new ()
    {
        UserName = "JohnDoe1337",
        Password = "P4S$W0rD123",
        ForeName = "John",
        LastName = "Doe",
        Active = true,
        Age = 42
    };

    public static User InvalidUser = new ()
    {
        UserName = "JohnDoe",
        Password = "P4S$W0r",
        ForeName = "J",
        LastName = "D",
        Age = 16
    };

    [Required]
    [MinLength(8)]
    [MaxLength(30)]
    public string UserName { get; set; } = string.Empty;

    // password must contain minimum of eight characters with at least one letter and one number
    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string ForeName { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    [MaxLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public bool Active { get; set; } = true;

    [Required]
    [Range(18, 130)]
    public int Age { get; set; }
}