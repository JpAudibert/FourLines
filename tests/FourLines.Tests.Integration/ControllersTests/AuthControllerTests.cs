using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.Users.Request;
using FourLines.Tests.Integration.Shared;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class AuthControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task Authenticate_WithValidCredentials_ReturnsOk()
    {
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/auth",
            method: HttpMethod.Post,
            content: JsonContent.Create(
                new LoginRequest { Email = "jane.smith@example.com", Password = "123" }
            )
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Authenticate_WithInvalidCredentials_ReturnsUnauthorized()
    {
        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/auth",
            method: HttpMethod.Post,
            content: JsonContent.Create(
                new LoginRequest { Email = "jane.smith@example.com", Password = "invalid" }
            )
        );

        Assert.Equal(StatusCodes.Status401Unauthorized, response.StatusCode);
    }
}
