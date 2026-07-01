using FourLines.Application.DependencyInjection;
using FourLines.Domain.DependencyInjection;
using FourLines.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FourLines.Tests;

public class InMemoryFixtures
{
    public IConfiguration Configuration { get; set; }
    public HostApplicationBuilder Builder { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    
    public InMemoryFixtures()
    {
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Tests.json").AddInMemoryCollection().Build();

        Builder = new HostApplicationBuilder();

        Builder.Services
            .AddInfrastructure(Configuration)
            .AddApplication()
            .AddDomain();

        Builder.Configuration.AddConfiguration(Configuration);
        
        ServiceProvider = Builder.Services.BuildServiceProvider();

        Builder.Build();
    }
}