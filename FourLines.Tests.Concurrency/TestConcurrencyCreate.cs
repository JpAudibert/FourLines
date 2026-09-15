using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Handlers;
using FourLines.Application.Interfaces;
using FourLines.Application.Strategies;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Concurrency.Seed;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;

namespace FourLines.Tests.Concurrency;

public class TestConcurrencyCreate(PostgresTestDatabase database)
    : IClassFixture<PostgresTestDatabase>
{
    private readonly PostgresTestDatabase _database = database;

    [Fact]
    public async Task ShouldNot_AllowTwoConcurrentReservations()
    {
        // Arrange
        Mock<IReservationValidator> validator = new();
        validator
            .Setup(v =>
                v.ValidateAsync(It.IsAny<CreateReservationDTO>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(
                (CreateReservationDTO dto, CancellationToken _) =>
                    Result<ConfirmReservationResponseDTO>.Success(
                        new ConfirmReservationResponseDTO
                        {
                            Match = default!,
                            Reservation = new Reservation
                            {
                                Id = Guid.NewGuid(),
                                CourtId = dto.CourtId,
                                UserId = dto.UserId,
                                Period = dto.Period,
                            },
                        }
                    )
            );

        async Task<Result<ConfirmReservationResponseDTO>> MakeReservation()
        {
            await using var reservationContext = _database.CreateContext();

            IReservationHandler reservationHandler = new ReservationHandler(
                reservationContext,
                validator.Object,
                new PostgresCourtLockStrategy(reservationContext)
            );

            try
            {
                return await reservationHandler.Create(
                    new CreateReservationDTO
                    {
                        CourtId = CourtSeed.Default.Id,
                        UserId = UserSeed.Player.Id,
                        Period = new TimeRange(
                            TestDates.Future.AddHours(1),
                            TestDates.Future.AddHours(2)
                        ),
                        Price = new Money(50.00m, "BRL"),
                    }
                );
            }
            catch (PostgresException)
            {
                return Result<ConfirmReservationResponseDTO>.Failure(
                    new Error("Failed to create reservation.")
                );
            }
        }

        // Act
        Result<ConfirmReservationResponseDTO>[] results = await Task.WhenAll(
            MakeReservation(),
            MakeReservation()
        );

        // Assert
        Result<ConfirmReservationResponseDTO> result1 = results[0];
        Result<ConfirmReservationResponseDTO> result2 = results[1];

        Assert.True(result1.IsSuccess ^ result2.IsSuccess, "Only one reservation should succeed.");

        await using var verificationContext = _database.CreateContext();

        List<Reservation> reservations = await verificationContext
            .Reservations.Where(x => x.CourtId == CourtSeed.Default.Id)
            .ToListAsync();

        Assert.Single(reservations);
    }
}
