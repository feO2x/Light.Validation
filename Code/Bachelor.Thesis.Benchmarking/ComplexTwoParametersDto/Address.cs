namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class Address
{
    public static Address ValidAddress = new ()
    {
        Country = "Germany",
        City = "Regensburg",
        Region = "Bavaria",
        PostalCode = 93053,
        Street = "Seybothstraße 2"
    };

    public string City { get; set; } = String.Empty;

    public string Country { get; set; } = String.Empty;

    public int PostalCode { get; set; }

    public string Region { get; set; } = String.Empty;

    public string Street { get; set; } = String.Empty;
}