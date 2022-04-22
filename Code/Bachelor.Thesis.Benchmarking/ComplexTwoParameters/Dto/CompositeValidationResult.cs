using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.ComplexTwoParameters.Dto;

public class CompositeValidationResult : ValidationResult
{
    private readonly List<ValidationResult> _results = new ();

    public IEnumerable<ValidationResult> Results =>
        _results;

    public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
    public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
    protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

    public void AddResult(ValidationResult validationResult)
    {
        _results.Add(validationResult);
    }
}