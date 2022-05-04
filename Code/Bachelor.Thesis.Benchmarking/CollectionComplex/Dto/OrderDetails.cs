using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class OrderDetails
{
    public static OrderDetails ValidOrderDetails = new ()
    {
        OrderId = 10010,
        ProductId = 20010,
        Date = DateTime.Parse("2022-04-27"),
        Quantity = 20,
        PricePaid = 100
    };

    public static OrderDetails InvalidOrderDetails = new()
    {
        OrderId = 1001,
        ProductId = 2001,
        Date = DateTime.Parse("2023-04-27"),
        Quantity = -8,
        PricePaid = 2000000
    };

    [Required]
    [Range(10000, long.MaxValue)]
    public long OrderId { get; set; }

    [Required]
    [Range(10000, long.MaxValue)]
    public long ProductId { get; set; }

    [Required]
    [Range(typeof(DateTime), "2020-01-01", "2022-12-31")]
    public DateTime Date { get; set; }

    [Required]
    [Range(0, 1000)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, 100000.0)]
    public decimal PricePaid { get; set; }
}