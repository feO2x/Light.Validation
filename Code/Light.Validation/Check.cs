namespace Light.Validation;

public readonly record struct Check<T>(ValidationContext Context, string Key, T Value);