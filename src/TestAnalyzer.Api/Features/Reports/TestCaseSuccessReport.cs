using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Testiny.Client;
using Testiny.Client.Serialization;

namespace TestAnalyzer.Api.Features.Reports;

public class TestCaseMetrics
{
    [JsonConverter(typeof(TestinyIdConverter<TestCaseId>))]
    public TestCaseId TestCaseId { get; init; }
    public int TestCaseRouteId => TestCaseId;
    public string Project { get; init; }
    public string Title { get; init; }
    public int Runs { get; init; } = 0;
    public int Failed { get; init; } = 0;
    public int Skipped { get; init; } = 0;
    public int NotRun { get; init; } = 0;
    public int Passed { get; init; } = 0;
    public int Blocked { get; init; } = 0;
    public float SuccessRate { get; init; } = 100;
}

public class TestCaseSuccessRateReportHandler
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<TestCaseSuccessRateReportHandler> _logger;
    private readonly TestinyClient _client;
    
    const string FileName = "TestCaseSuccessRateReportHandler.json";
    static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public TestCaseSuccessRateReportHandler(IMemoryCache cache,  ILogger<TestCaseSuccessRateReportHandler> logger, TestinyClient client)
    {
        _cache = cache;
        _logger = logger;
        _client = client;
        
        PersistenceSerializationOptions = new JsonSerializerOptions
        {
            AllowOutOfOrderMetadataProperties = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        PersistenceSerializationOptions.Converters.Add(new TestinyIdConverter<TestCaseId>());
        PersistenceSerializationOptions.Converters.Add(new TestinyIdConverter<TestRunId>());
    }
    
    public async Task<IEnumerable<TestCaseMetrics>> HandleAsync(CancellationToken token = default)
    {
        return await _cache.GetOrCreateAsync("TestCaseSuccessRateReportHandler.HandleAsync", (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return GetData(token);
        }) ?? Array.Empty<TestCaseMetrics>();
    }

    internal JsonSerializerOptions PersistenceSerializationOptions { get; } = new();
    
    /// <summary>
    /// Crude File Persistence
    /// better would be a background data loader
    /// </summary>
    private async Task<IEnumerable<TestCaseMetrics>> GetData(CancellationToken token = default)
    {
        if (File.Exists(FileName))
        {
            _logger.LogInformation("Reading file {FileName} created: {LastWriteTimeUtc}", FileName, File.GetLastWriteTimeUtc(FileName).ToString("o"));
            var db = File.OpenRead(FileName);
            
            var deserialized = await JsonSerializer.DeserializeAsync<IEnumerable<TestCaseMetrics>>(db, PersistenceSerializationOptions);
            if(deserialized != null)
                return deserialized; //short circuit
        }
        
        var report = await BuildReport(token);

        try
        {
            var stream = File.OpenWrite(FileName);
            await JsonSerializer.SerializeAsync(stream, report, PersistenceSerializationOptions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create file {FileName}", FileName);
            throw;
        }

        return report;
    }
    
    private async Task<IEnumerable<TestCaseMetrics>> BuildReport(CancellationToken cancellationToken = default)
    {
        var metrics = new ConcurrentBag<SimplifiedTestCase>();
        var start = Stopwatch.GetTimestamp();
        
        var projects = await GetProjects(cancellationToken);

        await foreach (var run in _client.GetTestRuns(cancellationToken))
        {
            await foreach (var result in _client.GetTestResultsForTestRun(run.Id, cancellationToken))
            {
                var projectKey = projects.ContainsKey(result.ProjectId) ? projects[result.ProjectId].ProjectKey : "";
                metrics.Add(new SimplifiedTestCase(run.Id, result.TestrunTestcaseValues.TestcaseId, projectKey, result.Title, result.TestrunTestcaseValues.ResultStatus));
            }
        }
        
        _logger.LogInformation("Run time: {StopwatchElapsedMilliseconds} ms", Stopwatch.GetElapsedTime(start));
        
        return metrics.GroupBy(result => result.TestCase.ToString())
            .Select(g => new TestCaseMetrics()
            {
                TestCaseId = g.Key,
                Project = g.FirstOrDefault()?.ProjectKey ?? "",
                Title = g.FirstOrDefault()?.Title ?? "",
                Runs = g.Count(),
                Passed = g.Count(x => x.Status.Equals("passed", StringComparison.OrdinalIgnoreCase)),
                NotRun = g.Count(x => x.Status.Equals("notrun", StringComparison.OrdinalIgnoreCase)),
                Skipped = g.Count(x => x.Status.Equals("skipped", StringComparison.OrdinalIgnoreCase)),
                Blocked = g.Count(x => x.Status.Equals("blocked", StringComparison.OrdinalIgnoreCase)),
                Failed = g.Count(x => x.Status.Equals("failed", StringComparison.OrdinalIgnoreCase)),
                SuccessRate = (g.Count(x => x.Status.Equals("passed", StringComparison.OrdinalIgnoreCase)) /
                               (float)g.Count()) * 100
            }).OrderByDescending(x => x.Runs);
    }

    private async Task<IReadOnlyDictionary<int, Project>> GetProjects(CancellationToken cancellationToken)
    {
        var projects = new Dictionary<int, Project>();
        await foreach (var project in _client.GetProjects(cancellationToken))
        {
            projects.Add(project.Id, project);
        }
        return projects;
    }
}

record SimplifiedTestCase(TestRunId TestRun, TestCaseId TestCase, string ProjectKey, string Title, string Status); 

