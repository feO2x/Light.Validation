namespace Bachelor.Thesis.Benchmarking.ComplexTwoParametersDto;

public class Address
{
    public static Address Default = new ()
    {
        Country = "Germany",
        City = "Regensburg",
        Region = "Bavaria",
        PostalCode = 93053,
        Street = "Seybothstraße 2"
    };

    public string? City { get; set; }

    public string? Country { get; set; }

    public long PostalCode { get; set; }

    public string? Region { get; set; }

    public string? Street { get; set; }
}