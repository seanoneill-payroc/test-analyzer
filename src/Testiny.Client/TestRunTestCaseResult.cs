using System.Text.Json.Serialization;

namespace Testiny.Client;

public class TestRunTestCaseResult
{
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
    [JsonPropertyName("testrun_id")]
    public int TestrunId { get; set; }
    [JsonPropertyName("testcase_id")]
    public int TestcaseId { get; set; }
    [JsonPropertyName("testcase_source_query_id")]
    public object TestcaseSourceQueryId { get; set; }
    [JsonPropertyName("testcase_source_folder_id")]
    public object TestcaseSourceFolderId { get; set; }
    [JsonPropertyName("assigned_user_id")]
    public object AssignedUserId { get; set; }
    [JsonPropertyName("assigned_to")]
    public string AssignedTo { get; set; }
    [JsonPropertyName("$assignee_name")]
    public string AssigneeName { get; set; }
    [JsonPropertyName("result_status")]
    public string ResultStatus { get; set; }
    [JsonPropertyName("result_per_step")]
    public ResultPerStep[] ResultPerStep { get; set; }
    [JsonPropertyName("result_at")]
    public string ResultAt { get; set; }
    public int? ResultBy { get; set; }
    [JsonPropertyName("$comment_count")]
    public int CommentCount { get; set; }
    [JsonPropertyName("$workitem_count")]
    public int WorkitemCount { get; set; }
}