using FourLines.Api.Controllers;
using FourLines.Api.ViewModels.Users;
using FourLines.Application.DTOs;
using FourLines.Application.Handlers;
using FourLines.Application.Interfaces;
using FourLines.Domain.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;
using Moq;

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
        Mock<ILogger<UsersController>> mockUserRegisterLogger = new();
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
        IUserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<IUserHandler>();
        ITokenProvider jwtTokenProvider =
            fixtures.ServiceProvider.GetRequiredService<ITokenProvider>();

        IAuthenticationHandler handler =
            fixtures.ServiceProvider.GetRequiredService<IAuthenticationHandler>();

        UsersController UsersController = new(mockUserRegisterLogger.Object, userHandler);
        AuthController authController = new(mockAuthLogger.Object, handler);

        // Act
        ActionResult<User> userRegisterResult = await UsersController.Register(
            RoleSeed.Player.Id,
            newUser,
            CancellationToken.None
        );
        ActionResult<string> authResult = await authController.Authenticate(
            loginRequest,
            CancellationToken.None
        );

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
            RoleId = RoleSeed.Owner.Id,
        };

        IUserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<IUserHandler>();

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

        IUserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<IUserHandler>();

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

        IAuthenticationHandler authHandler =
            fixtures.ServiceProvider.GetRequiredService<IAuthenticationHandler>();

        // Act
        Result<string> result = await authHandler.Authenticate(authTest);

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
            Email = UserSeed.Player.Email,
            Password = "testingPassword",
        };

        IAuthenticationHandler authHandler =
            fixtures.ServiceProvider.GetRequiredService<IAuthenticationHandler>();

        // Act
        Result<string> result = await authHandler.Authenticate(authTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(AuthenticationErrorResults.InvalidPassword, result.Error);
    }
}
