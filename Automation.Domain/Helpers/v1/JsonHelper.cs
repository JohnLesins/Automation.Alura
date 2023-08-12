using Automation.Alura.Domain.Contracts.Helpers.v1;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Automation.Alura.Domain.Helpers.v1;

public class JsonHelper : IJsonHelper
{
    public async Task<string> SerializeAsync<T>(T @object, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        jsonSerializerOptions ??= new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            WriteIndented = false,
            MaxDepth = int.MaxValue,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        using var memoryStream = new MemoryStream();

        await JsonSerializer.SerializeAsync(memoryStream, @object, jsonSerializerOptions);

        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);

        return await reader.ReadToEndAsync();
    }
}