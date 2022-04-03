using System.Text.Json;

namespace Light.Validation.Tests.TestHelpers;

public static class Json
{
    public static JsonSerializerOptions Options { get; } = new ()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);
}