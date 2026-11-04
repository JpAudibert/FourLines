using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.Facilities;
using FourLines.Api.Models.Users.Request;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class FacilitiesControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        string token = await fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.Player.Email, Password = "123" }
        );

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/facilities",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMine_ReturnsOk()
    {
        string token = await OwnerTokenAsync();

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/facilities/mine",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsOk()
    {
        string token = await OwnerTokenAsync();

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.Default.Id}",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        string token = await OwnerTokenAsync();
        CreateFacilityRequest request = new()
        {
            Name = "Integration Test Facility",
            Address = "123 Integration Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "IT-FACILITY-001",
            TimeZoneId = "America/Sao_Paulo",
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/facilities",
            method: HttpMethod.Post,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        string token = await OwnerTokenAsync();
        UpdateFacilityRequest request = new()
        {
            Name = "Updated Integration Facility",
            Address = "456 Integration Test St",
            City = "Updated Test City",
            State = "TS",
            ZipCode = "54321",
            RegistrationNumber = "IT-FACILITY-002",
            TimeZoneId = "America/Sao_Paulo",
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.ToBeUpdated.Id}",
            method: HttpMethod.Put,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        string token = await OwnerTokenAsync();

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.ToBeDeleted.Id}",
            method: HttpMethod.Delete,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    private Task<string> OwnerTokenAsync() =>
        fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.Owner.Email, Password = "123" }
        );
}
