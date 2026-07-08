using FourLines.Api.Controllers;
using FourLines.Api.ViewModels.Users;
using FourLines.Application.DependencyInjection;
using FourLines.Application.DTOs;
using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.DependencyInjection;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FourLines.Tests;

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

        _connection = new SqliteConnection(Configuration.GetConnectionString("DefaultConnection"));

        _connection.Open();

        Builder.Services.AddSingleton(_connection);

        Builder.Services.AddInfrastructure(Configuration).AddApplication().AddDomain();

        Builder.Configuration.AddConfiguration(Configuration);

        ServiceProvider = Builder.Services.BuildServiceProvider();

        Builder.Build();

        using var scope = ServiceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async Task RemoveDataFromMemory<T>(Guid id)
        where T : BaseEntity
    {
        FourLinesContext context = ServiceProvider.GetRequiredService<FourLinesContext>();

        T entity = await context.Set<T>().FindAsync(id);

        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<T> CreateEntityInMemory<T>(T entity)
        where T : BaseEntity
    {
        FourLinesContext context = ServiceProvider.GetRequiredService<FourLinesContext>();

        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }
}
