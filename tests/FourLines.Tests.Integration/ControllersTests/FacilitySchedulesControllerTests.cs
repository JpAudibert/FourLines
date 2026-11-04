using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.FacilitySchedules;
using FourLines.Api.Models.Users.Request;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class FacilitySchedulesControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task GetSchedules_ReturnsOk()
    {
        string token = await OwnerTokenAsync();

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.Default.Id}/schedules",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        string token = await OwnerTokenAsync();
        CreateFacilityScheduleRequest request = new()
        {
            DayOfWeek = DayOfWeek.Monday,
            OpensAt = new TimeOnly(9, 0),
            ClosesAt = new TimeOnly(17, 0),
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.NoSchedules.Id}/schedules",
            method: HttpMethod.Post,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateMultiple_ReturnsOk()
    {
        string token = await OwnerTokenAsync();
        CreateFacilityScheduleRequest[] requests =
        [
            new()
            {
                DayOfWeek = DayOfWeek.Tuesday,
                OpensAt = new TimeOnly(9, 0),
                ClosesAt = new TimeOnly(17, 0),
            },
            new()
            {
                DayOfWeek = DayOfWeek.Wednesday,
                OpensAt = new TimeOnly(9, 0),
                ClosesAt = new TimeOnly(17, 0),
            },
        ];

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.NoSchedules2.Id}/schedules/multiple",
            method: HttpMethod.Post,
            content: JsonContent.Create(requests),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        string token = await OwnerTokenAsync();
        UpdateFacilityScheduleRequest request = new()
        {
            DayOfWeek = DayOfWeek.Friday,
            OpensAt = new TimeOnly(8, 0),
            ClosesAt = new TimeOnly(18, 0),
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/facilities/{FacilitySeed.NoSchedules.Id}/schedules/{FacilityScheduleSeed.ToBeUpdated.Id}",
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
            url: $"/api/v1/facilities/{FacilitySeed.NoSchedules.Id}/schedules/{FacilityScheduleSeed.ToBeDeleted.Id}",
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
