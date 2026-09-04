using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

public class TestReservationsCreate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly CreateReservationDTO _createReservationTest = new()
    {
        CourtId = InMemoryDataSource.Court1.Id,
        UserId = InMemoryDataSource.UserPlayer.Id,
        Period = new TimeRange(
            InMemoryDataSource.DateTime,
            InMemoryDataSource.DateTime.AddHours(1)
        ),
        Status = ReservationStatus.Pending,
    };

    [Fact]
    public async Task Should_CreateReservation()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(
                InMemoryDataSource.FacilitySchedule3,
                context
            );
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
        CreateReservationDTO _createReservationTestInvalidDate = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddHours(-2)),
        };
        CreateReservationDTO _createReservationTestInvalidPastDate = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now.AddHours(-2), DateTime.Now),
        };
        CreateReservationDTO _createReservationTestInvalidDayPeriod = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)),
        };
        CreateReservationDTO _createReservationTestInvalidDuration = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddHours(2)),
        };
        CreateReservationDTO _createReservationTestInvalidStatus = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(
                InMemoryDataSource.DateTime,
                InMemoryDataSource.DateTime.AddHours(1)
            ),
            Status = (ReservationStatus)999,
        };

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> resultDate = await reservationHandler.Create(
            _createReservationTestInvalidDate
        );
        Result<ConfirmReservationResponseDTO> resultPastDate = await reservationHandler.Create(
            _createReservationTestInvalidPastDate
        );
        Result<ConfirmReservationResponseDTO> resultDayPeriod = await reservationHandler.Create(
            _createReservationTestInvalidDayPeriod
        );
        Result<ConfirmReservationResponseDTO> resultDuration = await reservationHandler.Create(
            _createReservationTestInvalidDuration
        );
        Result<ConfirmReservationResponseDTO> resultStatus = await reservationHandler.Create(
            _createReservationTestInvalidStatus
        );

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
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Facility>(context);
            await DbOperations.RemoveAllDataFromMemory<FacilitySchedule>(context);
            await DbOperations.RemoveAllDataFromMemory<Court>(context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationUnknownCourt, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoUserFound()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.RemoveDataFromMemory<User>(InMemoryDataSource.UserPlayer.Id, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationUnknownUser, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoScheduleFound()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.RemoveAllDataFromMemory<FacilitySchedule>(context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationOutsideFacilitySchedule, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_OverlappingReservation()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(
                InMemoryDataSource.FacilitySchedule3,
                context
            );
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        CreateReservationDTO _createReservationTestOverlapping = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(
                InMemoryDataSource.DateTime.AddMinutes(30),
                InMemoryDataSource.DateTime.AddHours(1).AddMinutes(30)
            ),
        };

        // Act
        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(
            _createReservationTestOverlapping
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationOverlappingReservation, result.Error);
    }
}
