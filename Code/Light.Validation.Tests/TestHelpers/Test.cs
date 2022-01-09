namespace Light.Validation.Tests.TestHelpers;

public static class Test
{
    public static (Dto<T> dto, ValidationContext context) SetupDefault<T>(T value) =>
        (new Dto<T> { Value = value }, new ValidationContext());
}