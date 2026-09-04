using FourLines.Api.Controllers;
using FourLines.Api.ViewModels.Users;
using FourLines.Application.DTOs;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Users;

public class UsersRegisterAndAuthTests(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_RegisterAndAuthenticateUser()
    {
        // Arrange
        Mock<ILogger<AuthController>> mockAuthLogger = new();
        Mock<ILogger<UserRegisterController>> mockUserRegisterLogger = new();
        UserRegisterViewModel newUser = new()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
        };

        LoginViewModel loginRequest = new()
        {
            Email = "john.doe@example.com",
            Password = "Password123!",
        };

        FourLinesContext context = fixtures.ServiceProvider.GetRequiredService<FourLinesContext>();

        User? testUser = await context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (testUser is not null)
        {
            context.Users.Remove(testUser);
            await context.SaveChangesAsync();
        }

        Role? testRole = await context.Roles.FirstOrDefaultAsync(r =>
            r.Name == RoleConstants.Player
        );
        Guid roleGuid = Guid.NewGuid();
        if (testRole is null)
        {
            await context.Roles.AddAsync(new() { Id = roleGuid, Name = RoleConstants.Player });
            await context.SaveChangesAsync();
        }

        IPasswordHashProvider passwordHashProvider =
            fixtures.ServiceProvider.GetRequiredService<IPasswordHashProvider>();
        UserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<UserHandler>();
        ITokenProvider jwtTokenProvider =
            fixtures.ServiceProvider.GetRequiredService<ITokenProvider>();

        AuthenticationHandler authenticationHandler = new(
            context,
            passwordHashProvider,
            jwtTokenProvider
        );

        UserRegisterController userRegisterController = new(
            mockUserRegisterLogger.Object,
            userHandler
        );
        AuthController authController = new(mockAuthLogger.Object, authenticationHandler);

        // Act
        ActionResult<User> userRegisterResult = await userRegisterController.Register(
            roleGuid,
            newUser
        );
        ActionResult<string> authResult = await authController.Authenticate(loginRequest);

        // Assert
        Assert.NotNull(userRegisterResult.Value);
        Assert.IsType<User>(userRegisterResult.Value);
        Assert.IsType<string>(authResult.Value);
    }

    [Fact]
    public async Task Should_Not_HaveDuplicateUser()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
        }

        UserRegisterDTO createUserTest = new()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
            RoleId = InMemoryDataSource.RoleOwner.Id,
        };

        UserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<UserHandler>();

        // Act
        Result<User> result = await userHandler.Create(createUserTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(UsersErrorResults.EmailAlreadyExists, result.Error);
    }

    [Fact]
    public async Task Should_Not_HaveUserRole()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.RemoveAllDataFromMemory<User>(context);
        }

        UserRegisterDTO _createUserTest = new()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
            RoleId = Guid.NewGuid(),
        };

        UserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<UserHandler>();

        // Act
        Result<User> result = await userHandler.Create(_createUserTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(UsersErrorResults.InvalidRole, result.Error);
    }

    [Fact]
    public async Task Should_Not_HaveUserForAuthentication()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<User>(context);
        }

        AuthenticationDTO authTest = new() { Email = "test@test.com", Password = "Test123!" };

        AuthenticationHandler authHandler =
            fixtures.ServiceProvider.GetRequiredService<AuthenticationHandler>();

        // Act
        Result<String> result = await authHandler.Authenticate(authTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(AuthenticationErrorResults.UnknownUser, result.Error);
    }

    [Fact]
    public async Task Should_Not_HaveEqualPasswords()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
        }

        User userOwnerTest = new()
        {
            RoleId = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john.doe@example.com",
            PasswordHash = "VGVzdDEyMyEK",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
        };

        AuthenticationDTO authTest = new()
        {
            Email = userOwnerTest.Email,
            Password = userOwnerTest.PasswordHash,
        };

        AuthenticationHandler authHandler =
            fixtures.ServiceProvider.GetRequiredService<AuthenticationHandler>();

        // Act
        Result<String> result = await authHandler.Authenticate(authTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(AuthenticationErrorResults.InvalidPassword, result.Error);
    }
}
