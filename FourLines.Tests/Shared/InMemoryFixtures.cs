using FourLines.Application.DependencyInjection;
using FourLines.Domain.DependencyInjection;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FourLines.Tests.Shared;

public class InMemoryFixtures
{
    public IConfiguration Configuration { get; set; }
    public HostApplicationBuilder Builder { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    private readonly SqliteConnection _connection;

    public InMemoryFixtures()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Tests.json")
            .AddInMemoryCollection()
            .Build();

        Builder = new HostApplicationBuilder();

        _connection = new SqliteConnection(
            Configuration.GetConnectionString(
                $"Data Source={Guid.NewGuid()};Mode=Memory;Cache=Shared"
            )
        );

        _connection.Open();

        Builder.Services.AddSingleton(_connection);

        Builder.Services.AddInfrastructure(Configuration).AddApplication().AddDomain();

        Builder.Configuration.AddConfiguration(Configuration);

        IHost host = Builder.Build();

        ServiceProvider = host.Services;

        using var scope = ServiceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        try
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating in-memory database: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async Task RemoveAllDataFromMemory<T>()
        where T : BaseEntity
    {
        FourLinesContext context = ServiceProvider.GetRequiredService<FourLinesContext>();

        context.Set<T>().RemoveRange(context.Set<T>());
        await context.SaveChangesAsync();
    }

    public async Task RemoveDataFromMemory<T>(Guid id)
        where T : BaseEntity
    {
        FourLinesContext context = ServiceProvider.GetRequiredService<FourLinesContext>();

        T? entity = await context.Set<T>().FindAsync(id);

        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task<T> CreateEntityInMemory<T>(T entity)
        where T : BaseEntity
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        if (await context.FindAsync<T>(entity.Id) == null)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        return entity;
    }
}
