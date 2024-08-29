using GameStore.Api.Users.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Hosting;
using Npgsql;
using Testcontainers.PostgreSql;

namespace GameStore.Api.Users.Tests;

public class TestApp : AppFixture<TestHost>
{
    private const string dbImage = "postgres:16-alpine";
    private const string dbName = "gamestore_db";

    private PostgreSqlContainer _postgres = null!;
    
    protected override async Task PreSetupAsync()
    {
        _postgres = new PostgreSqlBuilder().WithImage(dbImage).WithDatabase(dbName+"_"+Guid.NewGuid()).Build();

        await _postgres.StartAsync();
        
        var result = await _postgres.ExecScriptAsync(await File.ReadAllTextAsync("TestData/data.sql"));
        if (result.ExitCode != 0 || result.Stderr.Length > 0)
        {
            throw new Exception("Failed to seed test data.");
        }
    }
    
    protected override Task SetupAsync()
    {
        // place one-time setup for the fixture here
        return Task.CompletedTask;
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        // do host builder configuration here
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        var connectionString = _postgres.GetConnectionString() + ";SearchPath=user";
        s.AddNpgsqlDataSource(connectionString);
    }

    protected override Task TearDownAsync()
    {
        // do cleanups here
        return Task.CompletedTask;
    }
}