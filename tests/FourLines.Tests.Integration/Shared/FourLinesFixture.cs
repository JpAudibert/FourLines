using DotNet.Testcontainers.Builders;
using FourLines.Api;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace FourLines.Tests.Integration.Shared;

public class FourLinesFixture : IAsyncLifetime
{
    public IConfiguration Configuration { get; set; } = default!;
    public WebApplicationBuilder Builder { get; set; } = default!;
    public IServiceProvider ServiceProvider { get; set; } = default!;
    public string BaseUrl { get; private set; } = default!;

    private const string _databaseName = "fourlines_test";
    private const string _username = "fourlines";
    private const string _password = "fourlines";
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase(_databaseName)
        .WithUsername(_username)
        .WithPassword(_password)
        .WithWaitStrategy(
            Wait.ForUnixContainer()
                .UntilCommandIsCompleted("pg_isready", "-U", _username, "-d", _databaseName)
        )
        .Build();

    private string ConnectionString => _postgres.GetConnectionString();
    private WebApplication _app = default!;

    public async Task InitializeAsync()
    {
        await InjectDependencies();
        await using var scope = CreateAsyncServiceScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        await TestDataSeeder.SeedAsync(context);
    }

    public async Task DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }

        await _postgres.DisposeAsync();
    }

    public async Task InjectDependencies()
    {
        await _postgres.StartAsync();

        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.IntegrationTests.json")
            .AddInMemoryCollection()
            .Build();
        Configuration["ConnectionStrings:DefaultConnection"] = ConnectionString;
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "IntegrationTests");
        
        _app = Program.CreateApp([], Configuration);

        _app.Urls.Add("http://localhost:5211");
        await _app.StartAsync();

        var server = _app.Services.GetRequiredService<IServer>();
        var addressFeature = server.Features.Get<IServerAddressesFeature>();

        BaseUrl = addressFeature!.Addresses.Single();
        ServiceProvider = _app.Services;
    }

    public AsyncServiceScope CreateAsyncServiceScope()
    {
        return ServiceProvider.CreateAsyncScope();
    }
}
