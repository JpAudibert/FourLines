using FourLines.Api.Controllers;
using FourLines.Api.ViewModels;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        FourLinesContext context = _fixtures.ServiceProvider.GetRequiredService<FourLinesContext>();

        User? testUser = await context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if(testUser is not null)
        {
            context.Users.Remove(testUser);
            await context.SaveChangesAsync();
        }

        Role? testRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == RoleConstants.Player);
        if (testRole is null)
        {
            await context.Roles.AddAsync(new() { Name = RoleConstants.Player });
            await context.SaveChangesAsync();
        }

        IPasswordHashProvider passwordHashProvider = _fixtures.ServiceProvider.GetRequiredService<IPasswordHashProvider>();
        UserHandler userHandler = _fixtures.ServiceProvider.GetRequiredService<UserHandler>();
        ITokenProvider jwtTokenProvider = _fixtures.ServiceProvider.GetRequiredService<ITokenProvider>();

        AuthenticationHandler authenticationHandler = new(context, passwordHashProvider, jwtTokenProvider);

        UserRegisterController userRegisterController = new(userHandler);
        AuthController authController = new(authenticationHandler);

        // Act 
        ActionResult<User> userRegisterResult = await userRegisterController.Register(newUser);
        ActionResult<string> authResult = await authController.Authenticate(loginRequest);

        // Assert
        Assert.NotNull(userRegisterResult.Value);
        Assert.IsType<User>(userRegisterResult.Value);
        Assert.IsType<string>(authResult.Value);
    }
}
