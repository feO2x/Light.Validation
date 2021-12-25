namespace Light.Validation;

/// <summary>
/// Represents the different behaviors that are
/// executed when multiple errors are added to the
/// <see cref="ValidationContext" /> under the same
/// key. You can configure this behavior in
/// <see cref="ValidationContextOptions.MultipleErrorsPerKeyBehavior" />.
/// </summary>
public enum MultipleErrorsPerKeyBehavior
{
    /// <summary>
    /// When this behavior is active, the new error will be
    /// appended to the existing error message, placing a
    /// newline character in between them. This is the default behavior.
    /// </summary>
    AppendWithNewLine,

    /// <summary>
    /// When this behavior is active, the previous error
    /// will be replaced with the new error.
    /// </summary>
    ReplaceError,

    /// <summary>
    /// When this behavior is active, the new error will be placed
    /// in a List&lt;T&gt; alongside the previous error(s).
    /// </summary>
    PlaceInList
}