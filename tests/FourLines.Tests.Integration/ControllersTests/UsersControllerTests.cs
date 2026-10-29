using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.Users.Request;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class UsersControllerTests(FourLinesFixture fixture)
{
    [Fact]
    [Trait("Register", "Facility Owner")]
    public async Task UsersRegister_FacilityOwner_ReturnsCreatedUser()
    {
        // Arrange
        UserRegisterRequest user = new()
        {
            Name = "Terezinha Pagliari",
            Email = "terezinha@pagliari.com",
            Password = "123456",
            Birthday = new DateOnly(1969, 11, 6),
            Phone = "54999453224",
            RegistrationNumber = "284.992.400-89",
            IsActive = true,
        };

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/users/facility-owners",
            method: HttpMethod.Post,
            content: JsonContent.Create(user)
        );

        // Assert
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
    }

    [Fact]
    [Trait("Register", "Player")]
    public async Task UsersRegister_Player_ReturnsCreatedUser()
    {
        // Arrange
        UserRegisterRequest user = new()
        {
            Name = "Davi Pagliari",
            Email = "davi@pagliari.com",
            Password = "123456",
            Birthday = new DateOnly(1971, 11, 6),
            Phone = "54999458224",
            RegistrationNumber = "152.738.010-60",
            IsActive = true,
        };

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/users/players",
            method: HttpMethod.Post,
            content: JsonContent.Create(user)
        );

        // Assert
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
    }

    [Fact]
    public async Task UsersRegister_Player_MethodNotAllowedOnProfileRoute()
    {
        // Arrange
        UserRegisterRequest user = new()
        {
            Name = "Davi Pagliari",
            Email = "davi@pagliari.com",
            Password = "123456",
            Birthday = new DateOnly(1971, 11, 6),
            Phone = "54999458224",
            RegistrationNumber = "152.738.010-60",
            IsActive = true,
        };

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/users",
            method: HttpMethod.Post,
            content: JsonContent.Create(user)
        );

        // Assert
        Assert.Equal(StatusCodes.Status405MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_ReturnsOk()
    {
        // Arrange
        string token = await fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.Player.Email, Password = "123" }
        );

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/users",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_WithRouteUserId_Returns404()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        string token = await fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.DummyAuth.Email, Password = "123" }
        );

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/users/{userId}",
            method: HttpMethod.Get,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ReturnsOk()
    {
        // Arrange
        UserUpdateRequest user = new()
        {
            Name = "Titcho Cohsul",
            Birthday = new DateOnly(1971, 11, 6),
            Phone = "54999321123",
            RegistrationNumber = "552.814.790-59",
            IsActive = true,
        };
        string token = await fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.PlayerToBeUpdated.Email, Password = "123" }
        );

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/users",
            method: HttpMethod.Put,
            content: JsonContent.Create(user),
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_WithRouteUserId_Returns404()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        UserUpdateRequest user = new()
        {
            Name = "Titcho Cohsul",
            Birthday = new DateOnly(1971, 11, 6),
            Phone = "54999321123",
            RegistrationNumber = "552.814.790-59",
            IsActive = true,
        };
        string token = await fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.DummyAuth.Email, Password = "123" }
        );

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/users/{userId}",
            method: HttpMethod.Put,
            content: JsonContent.Create(user),
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent()
    {
        // Arrange
        string token = await fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.PlayerToBeDeleted.Email, Password = "123" }
        );

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/users",
            method: HttpMethod.Delete,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status204NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_WithRouteUserId_Returns404()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        string token = await fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.DummyAuth.Email, Password = "123" }
        );

        // Act
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/users/{userId}",
            method: HttpMethod.Delete,
            token: token
        );

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }
}
