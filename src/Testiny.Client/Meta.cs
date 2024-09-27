using System.Text.Json.Serialization;

namespace Testiny.Client;

public class Meta
{
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}