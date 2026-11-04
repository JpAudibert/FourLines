using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.Reservations;
using FourLines.Api.Models.Users.Request;
using FourLines.Application.DTOs.Reservations;
using FourLines.Domain.Models;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.Shared;

internal static class ControllerTestHelpers
{
    private static int _reservationSequence;

    public static Task<string> LoginAsync(FourLinesFixture fixture, string email) =>
        fixture.HttpClient.LoginAsync(new LoginRequest { Email = email, Password = "123" });

    public static async Task<ConfirmReservationResponseDTO> CreateReservationAsync(
        FourLinesFixture fixture,
        string token
    )
    {
        int sequence = Interlocked.Increment(ref _reservationSequence);
        DateTimeOffset start = TestDates.Future.AddDays(sequence).AddHours(18);
        CreateReservationRequest request = new()
        {
            CourtId = CourtSeed.Default.Id,
            Period = new TimeRange(start, start.AddHours(1)),
            Status = ReservationStatus.Pending,
            Price = new()
            {
                Amount = 50.00m,
                Currency = "BRL",
            },
        };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: "/api/v1/reservations/users/me",
            method: HttpMethod.Post,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

        return await response.GetJsonAsync<ConfirmReservationResponseDTO>();
    }
}
