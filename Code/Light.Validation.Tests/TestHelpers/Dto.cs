namespace Light.Validation.Tests.TestHelpers;

public sealed class Dto<T>
{
    public T Value { get; init; } = default!;
}