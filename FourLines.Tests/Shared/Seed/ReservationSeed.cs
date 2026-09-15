using FourLines.Application.DTOs.Reservations;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared.Seed;

public static class ReservationSeed
{
    public static readonly Reservation Default = new()
    {
        CourtId = CourtSeed.Default.Id,
        UserId = UserSeed.Player.Id,
        Period = new TimeRange(TestDates.Now, TestDates.Now.AddHours(1)),
        Status = ReservationStatus.Pending,
        Price = new Money(50.00m, "BRL"),
    };

    public static readonly Reservation ToBeDeleted = new()
    {
        CourtId = CourtSeed.Default.Id,
        UserId = UserSeed.Player.Id,
        Period = new TimeRange(TestDates.Now.AddHours(1), TestDates.Now.AddHours(2)),
        Status = ReservationStatus.Pending,
        Price = new Money(50.00m, "BRL"),
    };

    public static readonly Reservation ToBeUpdated = new()
    {
        CourtId = CourtSeed.Default.Id,
        UserId = UserSeed.Player.Id,
        Period = new TimeRange(TestDates.Now.AddHours(2), TestDates.Now.AddHours(3)),
        Status = ReservationStatus.Pending,
        Price = new Money(50.00m, "BRL"),
    };

    public static readonly CreateReservationDTO CreateGoalKeeperReservationTest = new()
    {
        CourtId = CourtSeed.Default.Id,
        UserId = UserSeed.Player.Id,
        Period = new TimeRange(TestDates.Future.AddHours(14), TestDates.Future.AddHours(15)),
        Status = ReservationStatus.Pending,
        Price = new Money(50.00m, "BRL"),
    };

    public static readonly CreateReservationDTO CreateNoGoalKeeperReservationTest = new()
    {
        CourtId = CourtSeed.CourtWithSportWithoutGoalkeeper.Id,
        UserId = UserSeed.Player.Id,
        Period = new TimeRange(TestDates.Future.AddHours(14), TestDates.Future.AddHours(15)),
        Status = ReservationStatus.Pending,
        Price = new Money(50.00m, "BRL"),
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        context.Reservations.AddRange(Default, ToBeDeleted, ToBeUpdated);

        await context.SaveChangesAsync();
    }
}
