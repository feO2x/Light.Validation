using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;

public class CustomerDto
{
    [Required]
    public User User { get; set; } = new ();

    [Required]
    public Address Address { get; set; } = new ();
}