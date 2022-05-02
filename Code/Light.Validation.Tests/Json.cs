using System.Text.Json;

namespace Light.Validation.Tests;

public static class Json
{
    private static JsonSerializerOptions IndentedOptions { get; } = new ()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, IndentedOptions);
}