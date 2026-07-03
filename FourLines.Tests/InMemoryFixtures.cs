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

    public InMemoryFixtures()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Tests.json")
            .AddInMemoryCollection()
            .Build();

        Builder = new HostApplicationBuilder();

        Builder.Services.AddInfrastructure(Configuration).AddApplication().AddDomain();

        Builder.Configuration.AddConfiguration(Configuration);

        ServiceProvider = Builder.Services.BuildServiceProvider();

        Builder.Build();
    }

    public async Task<T> CreateEntityInMemory<T>(T entity)
        where T : BaseEntity
    {
        FourLinesContext context = ServiceProvider.GetRequiredService<FourLinesContext>();

        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public UserRegisterDTO CreateUserRegisterDTO()
    {
        return new UserRegisterDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
            RoleId = Guid.NewGuid(),
        };
    }

    public CreateFacilityDTO CreateCreateFacilityDTO()
    {
        return new CreateFacilityDTO
        {
            OwnerId = Guid.NewGuid(),
            Name = "Test Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "12.345.678/0001-90",
        };
    }

    public UpdateFacilityDTO CreateUpdateFacilityDTO()
    {
        return new UpdateFacilityDTO
        {
            Name = "Test Facility Updated",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "12.345.678/0001-90",
        };
    }

    public UserRegisterViewModel CreateUserRegisterViewModel()
    {
        return new()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
        };
    }

    public LoginViewModel CreateUserLoginViewModel()
    {
        return new() { Email = "john.doe@example.com", Password = "Password123!" };
    }
}
