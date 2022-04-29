using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class CollectionComplexDto
{
    public static CollectionComplexDto ValidDto = new ()
    {
        OrderDetailsList = new ()
        {
            OrderDetails.ValidOrderDetails,
            OrderDetails.ValidOrderDetails
        },
        ArticleList = new ()
        {
            Article.ValidArticle,
            Article.ValidArticle,
            Article.ValidArticle,
            Article.ValidArticle,
            Article.ValidArticle,
            Article.ValidArticle
        }
    };

    public static CollectionComplexDto InvalidDto = new()
    {
        OrderDetailsList = new()
        {
            OrderDetails.InvalidOrderDetails,
            OrderDetails.InvalidOrderDetails
        },
        ArticleList = new()
        {
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
        }
    };

    [Required]
    [MinLength(1), MaxLength(10)]
    public List<OrderDetails> OrderDetailsList { get; set; } = new ();

    [Required]
    [MinLength(1), MaxLength(10)]
    public List<Article> ArticleList { get; set; } = new ();
}