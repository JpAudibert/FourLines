using FourLines.Api.Controllers;
using FourLines.Api.ViewModels;
using FourLines.Application.Handlers;
using FourLines.Application.Providers;
using FourLines.Domain.Constants;
using FourLines.Domain.Interfaces;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace FourLines.Tests;

public class UsersRegisterAndAuthTests : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures;
    public UsersRegisterAndAuthTests(InMemoryFixtures fixtures)
    {
        _fixtures = fixtures;
    }

    [Fact]
    public async Task UsersRegisterAndAuthTests_CRUD_Workflow()
    {
        // Arrange
        UserRegisterViewModel newUser = new()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
            RoleName = RoleConstants.Player,
            isActive = true
        };

        LoginViewModel loginRequest = new()
        {
            Email = "john.doe@example.com",
            Password = "Password123!"
        };

        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
        FourLinesContext context = _fixtures.CreateInMemoryContext("StandardRepoDb");

        StandardRepository<User> userRepository = new(context, new NullLogger<StandardRepository<User>>());
        IEnumerable<User> users = await userRepository.GetAllAsync();
        users.Where(u => u.Email == newUser.Email).ToList().ForEach(async u => await userRepository.DeleteAsync(u.Id));
        await userRepository.SaveChangesAsync();
        
        StandardRepository<Role> roleRepository = new(context, new NullLogger<StandardRepository<Role>>());
        await roleRepository.AddAsync(new Role { Name = RoleConstants.Player });
        await roleRepository.AddAsync(new Role { Name = RoleConstants.Admin });
        await roleRepository.SaveChangesAsync();

        IPasswordHashProvider passwordHashProvider = _fixtures.ServiceProvider.GetRequiredService<IPasswordHashProvider>();
        UserHandler userHandler = _fixtures.ServiceProvider.GetRequiredService<UserHandler>();
        ITokenProvider jwtTokenProvider = _fixtures.ServiceProvider.GetRequiredService<ITokenProvider>();

        AuthenticationHandler authenticationHandler = new(context, passwordHashProvider, jwtTokenProvider);

        UserRegisterController userRegisterController = new(userHandler);
        AuthController authController = new(authenticationHandler);

        // Act 
        IActionResult userRegisterResult = await userRegisterController.Register(newUser);
        IActionResult authResult = await authController.Authenticate(loginRequest);

        // Assert
        Assert.IsType<OkObjectResult>(userRegisterResult);
        Assert.IsType<OkObjectResult>(authResult);
        

    }

}
