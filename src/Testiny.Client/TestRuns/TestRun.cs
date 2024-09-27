using System.Text.Json.Serialization;

namespace Testiny.Client;

public class TestRun
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("sort_index")]
    public long SortIndex { get; set; }
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
    [JsonPropertyName("owner_user_id")]
    public int OwnerUserId { get; set; }
    [JsonPropertyName("$owner_name")]
    public string OwnerName { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("template")]
    public string Template { get; set; }
    [JsonPropertyName("precondition_text")]
    public string PreconditionText { get; set; }
    [JsonPropertyName("content_text")]
    public string ContentText { get; set; }
    [JsonPropertyName("steps_text")]
    public object StepsText { get; set; }
    [JsonPropertyName("expected_result_text")]
    public object ExpectedResultText { get; set; }
    [JsonPropertyName("priority")]
    public object Priority { get; set; }
    [JsonPropertyName("testcase_type")]
    public object TestcaseType { get; set; }
    [JsonPropertyName("cf__testcasetype")]
    public string[] CfTestcasetype { get; set; }
    [JsonPropertyName("cf__tags")]
    public string[] CfTags { get; set; }
    [JsonPropertyName("cf__automation_type")]
    public object CfAutomationType { get; set; }
    [JsonPropertyName("cf__context")]
    public string CfContext { get; set; }
    [JsonPropertyName("cf__testingcollateral")]
    public string CfTestingcollateral { get; set; }
    [JsonPropertyName("testrun_testcase_values")]
    public TestRunTestCaseResult TestrunTestcaseValues { get; set; }
}