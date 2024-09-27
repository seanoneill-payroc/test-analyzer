using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Testiny.Client;

public class TestinyClient
{
    private readonly ILogger<TestinyClient> _logger;
    private readonly HttpClient _httpClient;

    public TestinyClient(ILogger<TestinyClient> logger, HttpClient httpClient, IOptions<TestinyConfig> testinyConfig)
    {
        httpClient.BaseAddress = new Uri(testinyConfig.Value.BaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", testinyConfig.Value.ApiKey);

        _logger = logger;
        _httpClient = httpClient;
    }
    
    public async IAsyncEnumerable<TestRunSummary> GetTestRuns(CancellationToken cancellationToken = default)
    {
        CollectionResponse<TestRunSummary>? collection = null;
        var count = 0;
        while (!cancellationToken.IsCancellationRequested && NeedsPaging(out var nextOffset, collection))
        {
            _logger.LogInformation("Getting test runs... with offset {nextOffset}", nextOffset);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "testrun/find");

            var body =
$$"""
{
    "pagination": {
        "offset": {{nextOffset}}
    },
    "includeTotalCount": true
}
""";
            var requestBody = new StringContent(body, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = requestBody;
            
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("GetTestRuns failed with status code {response.StatusCode}\n{content}", response.StatusCode, content);
                yield break;
            }
            
            collection = await response.Content.ReadFromJsonAsync<CollectionResponse<TestRunSummary>>(cancellationToken: cancellationToken);
            foreach (var item in collection?.Data)
            {
                Interlocked.Increment(ref count);
                yield return item;
            }
        }
        _logger.LogInformation("{Method} finished with {Count} results", nameof(GetTestRuns), count);
    }

    public async IAsyncEnumerable<Project> GetProjects(CancellationToken cancellationToken = default)
    {
        CollectionResponse<Project>? collection = null;
        int count = 0;
        while (!cancellationToken.IsCancellationRequested && NeedsPaging(out var nextOffset, collection))
        {
            _logger.LogInformation("Getting projects... with offset {nextOffset}", nextOffset);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "project/find");

            var body =
                $$"""
                  {
                      "pagination": {
                          "offset": {{nextOffset}}
                      },
                      "includeTotalCount": true
                  }
                  """;
            var requestBody = new StringContent(body, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = requestBody;
            
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("GetTestRuns failed with status code {response.StatusCode}\n{content}", response.StatusCode, content);
                yield break;
            }
            
            collection = await response.Content.ReadFromJsonAsync<CollectionResponse<Project>>(cancellationToken: cancellationToken);
            foreach (var item in collection?.Data)
            {
                Interlocked.Increment(ref count);
                yield return item;
            }
        }
        _logger.LogInformation("{Method} finished with {Count} results", nameof(GetProjects), count);
    }

    public async IAsyncEnumerable<TestRun> GetTestResultsForTestRun(TestRunId testRunId, CancellationToken cancellationToken = default)
    {
        var id = (int)testRunId;
        
        var json = 
            $$"""
              {
                "map":[ 
                  {
                      "entities": [ "testcase", "testrun" ],
                      "ids": {"testrun_id": {{id}} },
                      "resultIncludeMapping": true 
                  }
                ]
              } 
              """;
        
        var request = new HttpRequestMessage(HttpMethod.Post, $"testcase/find");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("GetTestRuns failed with status code {response.StatusCode}\n{content}", response.StatusCode, content);
            yield break;
        }
        

        var resultSet = await response.Content.ReadFromJsonAsync<CollectionResponse<TestRun>>();
        foreach (var result in resultSet?.Data)
            yield return result;
    }
    
    public Task<HttpResponseMessage> RawRequest(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        return _httpClient.SendAsync(request, cancellationToken);
    }
    
    private bool NeedsPaging<T>(out long nextOffset, CollectionResponse<T>? collection = null)
    {
        if (collection is null || collection?.Meta is null)
        {
            nextOffset = 0;
            return true;
        }
        
        Meta meta = collection.Meta;

        if (meta.TotalCount <= (meta.Offset + (collection?.Data.Length ?? 0)))
        {
            //we have all records
            nextOffset = 0;
            return false;
        }
        
        nextOffset = meta.Offset + collection.Data.Length;
        return true;
    }
}


// public record Project(
//     int Id,
//     string CreatedAt,
//     int CreatedBy,
//     string ModifiedAt,
//     int ModifiedBy,
//     object DeletedAt,
//     object DeletedBy,
//     string Etag,
//     int OwnerUserId,
//     string Name,
//     string ProjectKey,
//     string Description
// );