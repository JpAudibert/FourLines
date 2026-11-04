using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.Courts;
using FourLines.Api.Models.Users.Request;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class CourtsControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task GetAllFromFacility_ReturnsOk()
    {
        string token = await PlayerTokenAsync();

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.Default.Id}/courts",
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
            url: $"/api/v1/facilities/{FacilitySeed.Default.Id}/courts/{CourtSeed.Default.Id}",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        string token = await OwnerTokenAsync();
        CreateCourtRequest request = new()
        {
            Name = "Integration Test Court",
            SportId = SportSeed.Default.Id,
            IsActive = true,
            DefaultPrice = new(50.00m, "BRL"),
            RentingPeriodInMinutes = 60,
            MaintenancePeriodInMinutes = 0,
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.NoSchedules.Id}/courts",
            method: HttpMethod.Post,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_MissingRequiredFields_ReturnsBadRequest()
    {
        string token = await OwnerTokenAsync();
        var request = new
        {
            Name = "Integration Test Court",
            SportId = SportSeed.Default.Id,
            IsActive = true,
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.NoSchedules.Id}/courts",
            method: HttpMethod.Post,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        string token = await OwnerTokenAsync();
        UpdateCourtRequest request = new()
        {
            Name = "Updated Integration Court",
            SportId = SportSeed.Default.Id,
            IsActive = true,
            DefaultPrice = new(75.00m, "BRL"),
            RentingPeriodInMinutes = 90,
            MaintenancePeriodInMinutes = 15,
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.Default.Id}/courts/{CourtSeed.ToBeUpdated.Id}",
            method: HttpMethod.Put,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Update_MissingRequiredFields_ReturnsBadRequest()
    {
        string token = await OwnerTokenAsync();
        var request = new
        {
            Name = "Updated Integration Court",
            SportId = SportSeed.Default.Id,
            IsActive = true,
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.Default.Id}/courts/{CourtSeed.ToBeUpdated.Id}",
            method: HttpMethod.Put,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        string token = await OwnerTokenAsync();

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.Default.Id}/courts/{CourtSeed.ToBeDeleted.Id}",
            method: HttpMethod.Delete,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    private Task<string> OwnerTokenAsync() =>
        fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.Owner.Email, Password = "123" }
        );

    private Task<string> PlayerTokenAsync() =>
        fixture.HttpClient.LoginAsync(
            new LoginRequest { Email = UserSeed.Player.Email, Password = "123" }
        );
}
