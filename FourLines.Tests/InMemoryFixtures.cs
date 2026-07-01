using FourLines.Application.DependencyInjection;
using FourLines.Domain.DependencyInjection;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FourLines.Tests;

public class InMemoryFixtures
{
    public IConfigurationBuilder Configuration { get; set; }
    public HostApplicationBuilder Builder { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    
    public InMemoryFixtures()
    {
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Tests.json").AddInMemoryCollection();

        Builder = new HostApplicationBuilder();

        Builder.Services.AddApplication()
                        .AddDomain();

        Builder.Configuration.AddConfiguration(Configuration.Build());
        
        Builder.Build();

        ServiceProvider = Builder.Services.BuildServiceProvider();
    }

    public FourLinesContext CreateInMemoryContext(string dbName)
    {
        DbContextOptions<FourLinesContext> options = new DbContextOptionsBuilder<FourLinesContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new FourLinesContext(options) { Configuration = Configuration.Build() };
    }
}