using FourLines.Application.DTOs.Reservations;
using FourLines.Application.DTOs.Reservations.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

public record TestCreateReservationDTO : ICreateReservationDTO
{
    public Guid CourtId { get; init; }
    public TimeRange Period { get; init; } = default!;
    public ReservationStatus Status { get; init; }
    public Guid UserId { get; init; }
}

[Collection(ReservationsCollection.Name)]
public class TestReservationsCreate(ReservationsFixture fixtures)
{
    private static readonly TestCreateReservationDTO _createReservationTest = new()
    {
        CourtId = TestDataSource.DefaultCourt.Id,
        UserId = TestDataSource.UserPlayer.Id,
        Period = new TimeRange(
            TestDataSource.DateTimeNow.AddHours(1),
            TestDataSource.DateTimeNow.AddHours(2)
        ),
        Status = ReservationStatus.Pending,
    };

    [Fact]
    public async Task Should_CreateReservation()
    {
        // Arrange
        IReservationHandler reservationHandler = fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<ConfirmReservationResponseDTO>(result.Value);
        Assert.Equal(_createReservationTest.CourtId, result.Value.Reservation.CourtId);
        Assert.Equal(_createReservationTest.UserId, result.Value.Reservation.UserId);
        Assert.Equal(_createReservationTest.Period, result.Value.Reservation.Period);
        Assert.Equal(_createReservationTest.Status, result.Value.Reservation.Status);


        Assert.NotNull(result.Value.Match);
        Assert.Equal(6, result.Value.Match.Code.Length);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_RejectedValidation()
    {
        // Arrange
        TestCreateReservationDTO reservationWithInvalidDate = _createReservationTest with
        {
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddHours(-2)),
        };
        TestCreateReservationDTO reservationWithInvalidPastDate = _createReservationTest with
        {
            Period = new TimeRange(DateTime.Now.AddHours(-2), DateTime.Now),
        };
        TestCreateReservationDTO reservationWithInvalidDayPeriod = _createReservationTest with
        {
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)),
        };
        TestCreateReservationDTO reservationWithInvalidDuration = _createReservationTest with
        {
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddHours(2)),
        };
        TestCreateReservationDTO reservationWithInvalidStatus = _createReservationTest with
        {
            Status = (ReservationStatus)999,
        };

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> resultDate = await reservationHandler.Create(reservationWithInvalidDate);
        Result<ConfirmReservationResponseDTO> resultPastDate = await reservationHandler.Create(reservationWithInvalidPastDate);
        Result<ConfirmReservationResponseDTO> resultDayPeriod = await reservationHandler.Create(reservationWithInvalidDayPeriod);
        Result<ConfirmReservationResponseDTO> resultDuration = await reservationHandler.Create(reservationWithInvalidDuration);
        Result<ConfirmReservationResponseDTO> resultStatus = await reservationHandler.Create(reservationWithInvalidStatus);

        // Assert
        Assert.Null(resultDate.Value);
        Assert.Equal(ReservationsErrorResults.CreationInvalidDates, resultDate.Error);
        Assert.Null(resultPastDate.Value);
        Assert.Equal(ReservationsErrorResults.CreationStartAndEndInThePast, resultPastDate.Error);
        Assert.Null(resultDayPeriod.Value);
        Assert.Equal(
            ReservationsErrorResults.CreationStartAndEndNotInTheSameDay,
            resultDayPeriod.Error
        );
        Assert.Null(resultDuration.Value);
        Assert.Equal(
            ReservationsErrorResults.CreationDurationTimeDifferentThanConfiguration,
            resultDuration.Error
        );
        Assert.Null(resultStatus.Value);
        Assert.Equal(ReservationsErrorResults.CreationInvalidStatus, resultStatus.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoCourtFound()
    {
        // Arrange
        TestCreateReservationDTO reservationWithInvalidCourt = _createReservationTest with
        {
            CourtId = Guid.NewGuid(),
        };

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(reservationWithInvalidCourt);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationUnknownCourt, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoUserFound()
    {
        // Arrange
        TestCreateReservationDTO reservationWithInvalidUser = _createReservationTest with
        {
            UserId = Guid.NewGuid(),
        };

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(reservationWithInvalidUser);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationUnknownUser, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoScheduleFound()
    {
        // Arrange
        DateTimeOffset testDateTime = new(DateOnly.FromDateTime(DateTime.Today), new TimeOnly(22, 0), TimeSpan.Zero);
        TestCreateReservationDTO reservationWithSchedule = _createReservationTest with
        {
            CourtId = TestDataSource.Court3.Id,
            Period = new TimeRange(TestDataSource.DateTimeNow, TestDataSource.DateTimeNow.AddHours(1)),
        };

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(reservationWithSchedule);

        // AssertDefaultFacilitySchedule
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationOutsideFacilitySchedule, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_OverlappingReservation()
    {
        // Arrange
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        TestCreateReservationDTO reservationWithOverlapping = _createReservationTest with
        {
            Period = new TimeRange(
                TestDataSource.DateTimeNow.AddMinutes(30),
                TestDataSource.DateTimeNow.AddHours(1).AddMinutes(30)
            ),
        };

        await reservationHandler.Create(reservationWithOverlapping);

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(
            reservationWithOverlapping
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationOverlappingReservation, result.Error);
    }
}
