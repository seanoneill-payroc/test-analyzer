using System.Text.Json.Serialization;

namespace Testiny.Client;

public class CollectionResponse<T>
{
    [JsonPropertyName("meta")]
    public Meta Meta { get; set; }
    [JsonPropertyName("data")]
    public T[] Data { get; set; }
}