using Serilog;
using TestAnalyzer.Api.Features.Reports;
using Testiny.Client;

Log.Logger = new LoggerConfiguration().WriteTo.Console().Enrich.FromLogContext().CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSerilog();

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.Configure<TestinyConfig>(builder.Configuration.GetSection("TestinyConfig"));
builder.Services.AddHttpClient<TestinyClient>();
builder.Services.AddTransient<TestCaseSuccessRateReportHandler>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapGet("/api/testcasereport",
    (TestCaseSuccessRateReportHandler handler, CancellationToken cancellationToken = default) =>
        handler.HandleAsync(cancellationToken));

app.Run();

