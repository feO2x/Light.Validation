using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class CollectionComplexDto : IValidatableObject
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

    public static CollectionComplexDto InvalidDto = new ()
    {
        OrderDetailsList = new ()
        {
            OrderDetails.InvalidOrderDetails,
            OrderDetails.InvalidOrderDetails
        },
        ArticleList = new ()
        {
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle,
            Article.InvalidArticle
        }
    };

    [Required]
    [MinLength(1), MaxLength(10)]
    public List<OrderDetails> OrderDetailsList { get; set; } = new ();

    [Required]
    [MinLength(1), MaxLength(10)]
    public List<Article> ArticleList { get; set; } = new ();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var resultsOrderDetailsList = new List<ValidationResult>();
        var resultsArticleList = new List<ValidationResult>();
        var results = new List<ValidationResult>();

        foreach (var order in OrderDetailsList)
        {
            Validator.TryValidateObject(order, new ValidationContext(order), resultsOrderDetailsList, true);
            results.AddRange(resultsOrderDetailsList);
        }

        foreach (var article in ArticleList)
        {
            Validator.TryValidateObject(article, new ValidationContext(article), resultsArticleList, true);
            results.AddRange(resultsArticleList);
        }

        return results;
    }
}