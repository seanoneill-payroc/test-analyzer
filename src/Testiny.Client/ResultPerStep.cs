using System.Text.Json.Serialization;

namespace Testiny.Client;

public class ResultPerStep
{
    [JsonPropertyName("idx")]
    public int Index { get; set; }
    [JsonPropertyName("rid")]
    public string ResultId { get; set; }
    [JsonPropertyName("Result")]
    public string Result { get; set; }
}