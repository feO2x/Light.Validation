using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ParametersComplexTwo.Dto;

public class CustomerDto : IValidatableObject
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

    [Required]
    public User User { get; set; } = new ();

    [Required]
    public Address Address { get; set; } = new ();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var resultsUser = new List<ValidationResult>();
        var resultsAddress = new List<ValidationResult>();
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(User, new ValidationContext(User), resultsUser);
        results.AddRange(resultsUser);

        Validator.TryValidateObject(Address, new ValidationContext(Address), resultsAddress);
        results.AddRange(resultsAddress);

        return results;
    }
}