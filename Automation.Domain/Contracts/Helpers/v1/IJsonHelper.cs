using System.Text.Json;

namespace Automation.Alura.Domain.Contracts.Helpers.v1;

public interface IJsonHelper
{
    Task<string> SerializeAsync<T>(T @object, JsonSerializerOptions? jsonSerializerOptions = null);
}