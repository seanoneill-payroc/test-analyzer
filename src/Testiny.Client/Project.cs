using System.Text.Json.Serialization;

namespace Testiny.Client;

public record Project(
    [property:JsonPropertyName("id")]
    int Id,
    [property:JsonPropertyName("created_at")]
    string CreatedAt,
    [property:JsonPropertyName("created_by")]
    int CreatedBy,
    [property:JsonPropertyName("modified_at")]
    string ModifiedAt,
    [property:JsonPropertyName("modified_by")]
    int ModifiedBy,
    [property:JsonPropertyName("deleted_at")]
    object DeletedAt,
    [property:JsonPropertyName("deleted_by")]
    object DeletedBy,
    [property:JsonPropertyName("_etag")]
    string Etag,
    [property:JsonPropertyName("owner_user_id")]
    int OwnerUserId,
    [property:JsonPropertyName("name")]
    string Name,
    [property:JsonPropertyName("project_key")]
    string ProjectKey,
    [property:JsonPropertyName("description")]
    string Description
);