using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class OrderDetails
{
    [Required]
    [Range(10000, long.MaxValue)]
    public long OrderId { get; set; }

    [Required]
    [Range(10000, long.MaxValue)]
    public long ProductId { get; set; }

    [Required]
    [Range(typeof(DateOnly), "2020-01-01", "2022-12-31")]
    public DateOnly Date { get; set; }

    [Required]
    [Range(0, 1000)]
    public int Quantity { get; set; }

    [Required]
    [Range(0, 100000)]
    public decimal PricePaid { get; set; }
}