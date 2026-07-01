using FourLines.Api.Controllers;
using FourLines.Api.ViewModels;
using FourLines.Application.Handlers;
using FourLines.Application.Providers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace FourLines.Tests;

public class UsersRegisterAndAuthTests
{
    private static FourLinesContext CreateInMemoryContext(string dbName)
    {
        DbContextOptions<FourLinesContext> options = new DbContextOptionsBuilder<FourLinesContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new FourLinesContext(options) { Configuration = new ConfigurationBuilder().AddInMemoryCollection().Build() };
    }

    [Fact]
    public async Task UsersRegisterAndAuthTests_CRUD_Workflow()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();
        FourLinesContext context = CreateInMemoryContext("StandardRepoDb");
        
        StandardRepository<Role> roleRepository = new(context, new NullLogger<StandardRepository<Role>>());
        await roleRepository.AddAsync(new Role { Name = RoleConstants.Player });
        await roleRepository.AddAsync(new Role { Name = RoleConstants.Admin });
        await roleRepository.SaveChangesAsync();

        PasswordHashProvider passwordHashProvider = new(new PasswordHasher<User>());
        UserHandler userHandler = new(context, passwordHashProvider);
        JwtTokenProvider jwtTokenProvider = new(configuration);

        AuthenticationHandler authenticationHandler = new(context, passwordHashProvider, jwtTokenProvider);

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

        UserRegisterController userRegisterController = new(userHandler);
        AuthController authController = new(authenticationHandler);

        // Act 
        IActionResult userRegisterResult = await userRegisterController.Register(newUser);
        IActionResult authResult = await authController.Authenticate(loginRequest);

        // Assert
        Assert.IsType<OkObjectResult>(userRegisterResult.Result);
        Assert.IsType<OkObjectResult>(authResult.Result);
        
        

    }

}
