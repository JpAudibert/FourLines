using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Handlers;
using FourLines.Application.Interfaces;
using FourLines.Application.Strategies;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;

namespace FourLines.Tests.Concurrency;

public class TestConcurrencyCreate(PostgresTestDatabase database) : IClassFixture<PostgresTestDatabase>
{
    private readonly PostgresTestDatabase _database = database;

    [Fact]
    public async Task ShouldNot_AllowTwoConcurrentReservations()
    {
        // Arrange
        Mock<IReservationValidator> validator = new();
        validator.Setup(v => v.ValidateAsync(It.IsAny<CreateReservationDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CreateReservationDTO dto, CancellationToken _) => Result<Reservation>.Success(new Reservation
            {
                Id = Guid.NewGuid(),
                CourtId = dto.CourtId,
                UserId = dto.UserId,
                Period = dto.Period
            }));

        await using (var context = _database.CreateContext())
        {
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.RoleOwner, context);
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.RolePlayer, context);
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.UserOwner, context);
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.UserPlayer, context);
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.Facility1, context);
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.TestSport, context);
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.Court1, context);
            await PostgresTestDatabase.CreateEntityInMemory(InMemoryDataSource.FacilitySchedule3, context);
        }

        async Task<Result<Reservation>> MakeReservation()
        {
            await using var reservationContext = _database.CreateContext();

            IReservationHandler reservationHandler =
                new ReservationHandler(
                    reservationContext,
                    validator.Object,
                    new PostgresCourtLockStrategy(reservationContext));

            try
            {
                return await reservationHandler.Create(new CreateReservationDTO
                {
                    CourtId = InMemoryDataSource.Court1.Id,
                    UserId = InMemoryDataSource.UserPlayer.Id,
                    Period = new TimeRange(
                        InMemoryDataSource.SettedDateTime.AddHours(1),
                        InMemoryDataSource.SettedDateTime.AddHours(2)
                    )
                });
            }
            catch (PostgresException)
            {
                return Result<Reservation>.Failure(new Error("Failed to create reservation."));
            }
        }

        // Act
        Result<Reservation>[] results = await Task.WhenAll(MakeReservation(), MakeReservation());

        // Assert
        Result<Reservation> result1 = results[0];
        Result<Reservation> result2 = results[1];

        Assert.True(result1.IsSuccess ^ result2.IsSuccess, "Only one reservation should succeed.");

        await using var verificationContext = _database.CreateContext();

        List<Reservation> reservations = await verificationContext.Reservations
                .Where(x => x.CourtId == InMemoryDataSource.Court1.Id)
                .ToListAsync();

        Assert.Single(reservations);
    }
}
