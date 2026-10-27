using System.Net.Http.Json;
using System.Text.Json;
using Flurl.Http;
using Flurl.Http.Testing;
using FourLines.Api.ViewModels.Users;
using FourLines.Tests.Integration.Shared;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.Users;

[Collection(FourLinesCollection.Name)]
public class UsersCreateTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task CreateUsers_FacilityOwner_ReturnsCreatedUser()
    {
        // Arrange
        var httpClient = new TestHttpClient(fixture.BaseUrl);
        UserRegisterViewModel user = new()
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
        IFlurlResponse response = await httpClient.SendAsync(
            url: "/api/v1/users/facility-owners",
            method: HttpMethod.Post,
            content: JsonContent.Create(user)
        );

        // Assert
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateUsers_Player_ReturnsCreatedUser()
    {
        // Arrange
        var httpClient = new TestHttpClient(fixture.BaseUrl);
        UserRegisterViewModel user = new()
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
        IFlurlResponse response = await httpClient.SendAsync(
            url: "/api/v1/users/players",
            method: HttpMethod.Post,
            content: JsonContent.Create(user)
        );

        // Assert
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateUsers_Player_404Route()
    {
        // Arrange
        var httpClient = new TestHttpClient(fixture.BaseUrl);
        UserRegisterViewModel user = new()
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
        IFlurlResponse response = await httpClient.SendAsync(
            url: "/api/v1/users",
            method: HttpMethod.Post,
            content: JsonContent.Create(user)
        );

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }
}
