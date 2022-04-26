namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class Article
{
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}