using System;
using System.Text.Json;

namespace Bachelor.Thesis.Tests.Validators;

public static class Json
{
    public static JsonSerializerOptions Options { get; } = new ()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    public static string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, Options);
    }
}