using System.Text.Json;

namespace RC.CA.WebApi.Tests.Xunit.Endpoints;

public static class JsonOptionConstants
{
    public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
}
