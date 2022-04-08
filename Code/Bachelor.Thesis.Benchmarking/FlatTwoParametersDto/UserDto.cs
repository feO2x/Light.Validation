using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.FlatTwoParametersDto;

public class UserDto
{
    public static UserDto ValidDto = new () { Id = 42, Name = "John Doe" };
    public static UserDto InvalidDto = new () { Id = -1, Name = "    J  " };

    [Required]
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(80)]
    public string Name { get; set; } = String.Empty;
}