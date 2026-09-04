using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FourLines.Tests.Concurrency;

public class PostgresTestDatabase : IAsyncLifetime
{
    private readonly IConfiguration _configuration;

    public PostgresTestDatabase()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Tests.json")
            .AddInMemoryCollection()
            .Build();
    }

    public async Task DisposeAsync()
    {
        await using var context = CreateContext();
        await context.Database.EnsureDeletedAsync();
    }

    public async Task InitializeAsync()
    {
        await using var context = CreateContext();

        await context.Database.MigrateAsync();
    }

    public FourLinesContext CreateContext()
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Postgres") ??
            _configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string not found.");

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
