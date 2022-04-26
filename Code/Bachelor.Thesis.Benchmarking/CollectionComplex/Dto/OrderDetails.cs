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
    public DateOnly Date { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal PricePaid { get; set; }
}