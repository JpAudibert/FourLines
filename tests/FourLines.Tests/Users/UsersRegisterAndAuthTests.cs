using FourLines.Application.DTOs;
using FourLines.Application.DTOs.Users;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Users;

[Collection(FourLinesCollection.Name)]
public class UsersRegisterAndAuthTests(FourLinesFixture fixtures)
{
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
        };

        IUserHandler userHandler = fixtures.ServiceProvider.GetRequiredService<IUserHandler>();

        // Act
        Result<User> result = await userHandler.Create(createUserTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(UsersErrorResults.EmailAlreadyExists, result.Error);
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
