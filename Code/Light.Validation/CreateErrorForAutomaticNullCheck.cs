namespace Light.Validation;

/// <summary>
/// This is the delegate type representing the creation of the error object.
/// This delegate is called when an automatic null-check failed.
/// </summary>
/// <param name="context">
/// The validation context of the current check. DO NOT add the error object to this context,
/// just return it. You can use the context to retrieve additional objects that you might need
/// to create the error object.
/// </param>
/// <param name="key">The key of the error. The key is already normalized if necessary.</param>
/// <param name="displayName">The human-readable display name of the value being null.</param>
/// <returns>The object that describes the value being null.</returns>
public delegate object CreateErrorForAutomaticNullCheck(ValidationContext context, string key, string displayName);