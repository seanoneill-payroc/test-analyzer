using System.Text.Json.Serialization;

namespace Testiny.Client;

public class TestRunSummary
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }
    [JsonPropertyName("created_by")]
    public int CreatedBy { get; set; }
    [JsonPropertyName("modified_at")]
    public string ModifiedAt { get; set; }
    [JsonPropertyName("modified_by")]
    public int ModifiedBy { get; set; }
    [JsonPropertyName("deleted_at")]
    public object DeletedAt { get; set; }
    [JsonPropertyName("deleted_by")]
    public object DeletedBy { get; set; }
    [JsonPropertyName("_etag")]
    public string Etag { get; set; }
    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }
    public int? TestplanId { get; set; }
    [JsonPropertyName("closed_at")]
    public string ClosedAt { get; set; }
    public int? ClosedBy { get; set; }
    [JsonPropertyName("is_closed")]
    public bool IsClosed { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
}