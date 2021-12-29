namespace Light.Validation.Tools;

/// <summary>
/// Represents the abstraction of the check struct that leaves out the generic value to be checked.
/// </summary>
public interface ICheck
{
    /// <summary>
    /// Gets the key that identifies errors for the value.
    /// </summary>
    string Key { get; }
    
    /// <summary>
    /// Gets the context that manages the errors dictionary.
    /// </summary>
    ValidationContext Context { get; }
    
    /// <summary>
    /// Gets the value indicating whether the context has errors for the key of this check instance.
    /// </summary>
    bool HasError { get; }

    /// <summary>
    /// Adds the specified error message to the context.
    /// </summary>
    void AddError(string errorMessage);
}