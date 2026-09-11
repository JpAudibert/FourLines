using DotNet.Testcontainers.Builders;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace FourLines.Tests.Concurrency;

public class PostgresTestDatabase : IAsyncLifetime
{
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

    public async Task DisposeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();

        await _postgres.StopAsync();
        await _postgres.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        await using var context = CreateContext();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public FourLinesContext CreateContext()
    {
        string connectionString = _postgres.GetConnectionString();

        DbContextOptions<FourLinesContext> options = new DbContextOptionsBuilder<FourLinesContext>()
            .EnableDetailedErrors()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new FourLinesContext(options);
    }

    public static async Task<T> CreateEntityInMemory<T>(T entity, FourLinesContext pgContext)
        where T : BaseEntity
    {
        if (await pgContext.FindAsync<T>(entity.Id) == null)
        {
            await pgContext.Set<T>().AddAsync(entity);
            await pgContext.SaveChangesAsync();
        }

        return entity;
    }
}
