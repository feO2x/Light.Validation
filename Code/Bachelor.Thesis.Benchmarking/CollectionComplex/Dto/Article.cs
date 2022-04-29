using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class Article
{
    public static Article ValidArticle = new ()
    {
        Id = 10021,
        Title = "Graphics Card",
        Price = new decimal(1049.00),
        Quantity = 15
    };

    public static Article InvalidArticle = new()
    {
        Id = 1002,
        Title = "CPU",
        Price = new decimal(45000),
        Quantity = 0
    };

    [Required]
    [Range(10000, long.MaxValue)]
    public long Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 10)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 10000.0)]
    public decimal Price { get; set; }

    [Required]
    [Range(1, 10000)]
    public int Quantity { get; set; }
}