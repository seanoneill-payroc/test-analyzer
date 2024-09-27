using Microsoft.Extensions.Logging.Testing;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace Testiny.Client.TestingClientTests;

public class ManualClientTests
{
    private readonly ITestOutputHelper _helper;

    private readonly IOptions<TestinyConfig> options = Options.Create<TestinyConfig>(new TestinyConfig()
    {
        BaseUrl = "https://app.testiny.io/api/v1/",
        ApiKey = ""
    });
    
    public ManualClientTests(ITestOutputHelper helper)
    {
        _helper = helper;
    }
    
    [Fact]
    public async Task Test1()
    {
        var fakeLogCollector = new FakeLogCollector();
        var fakeLogger = new FakeLogger<TestinyClient>(fakeLogCollector);
        
        var client = new TestinyClient(fakeLogger, new HttpClient(), options);
        
        foreach (var run in client.GetTestRuns().ToBlockingEnumerable().Take(2))
        {
            await foreach (var result in client.GetTestResultsForTestRun(run.Id))
            {
                _helper.WriteLine($"TR-{run.Id} TC-{result.Id} - \"{result.Title}\" - {result.TestrunTestcaseValues}");
            }
        }

        foreach (var lm in fakeLogCollector.GetSnapshot().Select(x => x.Message))
        {
            _helper.WriteLine(lm);
        }
    }

    [Fact]
    public async Task Projects()
    {
        var fakeLogCollector = new FakeLogCollector();
        var fakeLogger = new FakeLogger<TestinyClient>(fakeLogCollector);
        
        var client = new TestinyClient(fakeLogger, new HttpClient(), options);

        await foreach (var project in client.GetProjects())
        {
            _helper.WriteLine($"{project.Id}: {project.ProjectKey}");
        }
    }
}