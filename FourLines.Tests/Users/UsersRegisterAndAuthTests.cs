using FourLines.Api.Controllers;
using FourLines.Api.ViewModels.Users;
using FourLines.Application.DTOs;
using FourLines.Application.Handlers;
using FourLines.Domain.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Users;

[Collection(FourLinesCollection.Name)]
public class UsersRegisterAndAuthTests(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_RegisterAndAuthenticateUser()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

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

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        User? testUser = await context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (testUser is not null)
        {
            context.Users.Remove(testUser);
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
            TestDataSource.RolePlayer.Id,
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
        await using var scope = fixtures.CreateAsyncServiceScope();

        UserRegisterDTO createUserTest = new()
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
            RoleId = TestDataSource.RoleOwner.Id,
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
        await using var scope = fixtures.CreateAsyncServiceScope();

        UserRegisterDTO createUserTest = new()
        {
            Name = "John Doe",
            Email = "randomEmailTest@example.com",
            Password = "Password123!",
            Birthday = new DateOnly(1970, 1, 1),
            Phone = "55 54 9 9999-9999",
            RegistrationNumber = "383.975.210-89",
            RoleId = Guid.NewGuid(),
        };

        UserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<UserHandler>();

        // Act
        Result<User> result = await userHandler.Create(createUserTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(UsersErrorResults.InvalidRole, result.Error);
    }

    [Fact]
    public async Task Should_Not_HaveUserForAuthentication()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

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
        await using var scope = fixtures.CreateAsyncServiceScope();

        AuthenticationDTO authTest = new()
        {
            Email = TestDataSource.UserPlayer.Email,
            Password = "testingPassword",
        };

        AuthenticationHandler authHandler =
            fixtures.ServiceProvider.GetRequiredService<AuthenticationHandler>();

        // Act
        Result<string> result = await authHandler.Authenticate(authTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(AuthenticationErrorResults.InvalidPassword, result.Error);
    }
}
