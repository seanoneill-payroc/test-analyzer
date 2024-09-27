# Get A Testiny Api Key

login to app.testiny.io, under the profile menu, select 'settings', 'api keys'

Name your key, select all projects and an expiration, limit the api key to 'Viewer'

copy your key for use in the command below

# Get the Project

```bash
git clone https://github.com/seanoneill-payroc/test-analyzer.git
cd test-analyzer
dotnet user-secrets set "TestinyConfig:ApiKey" "<Testiny Api Key>" --project ./src/TestAnalyzer.Api/TestAnalyzer.Api.csproj 
dotnet run --project ./src/TestAnalyzer.Api/TestAnalyzer.Api.csproj

start http://localhost:5178/
```

the first request will take about 2 minutes to generate a report, there are ways to address that but out of scope for this proof.

the testiny api is good for 1100 read requests/min. this execution should not approach that.

# Stack 
.net9 host for api, and a svelte frontend servied via static file hosting

