namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class OrderDetails
{
    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public DateTime Date { get; set; }

    public int Quantity { get; set; }

    public decimal PricePaid { get; set; }
}