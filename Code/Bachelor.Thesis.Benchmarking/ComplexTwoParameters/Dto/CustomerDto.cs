using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;

public class CustomerDto
{
    public static CustomerDto ValidCustomerDto = new()
    {
        User = User.ValidUser, 
        Address = Address.ValidAddress
    };
    public static CustomerDto InvalidCustomerDto = new()
    {
        User = User.InvalidUser, 
        Address = Address.InvalidAddress
    };

    [ValidateObject]
    public User User { get; set; } = new ();

    [ValidateObject]
    public Address Address { get; set; } = new ();
}