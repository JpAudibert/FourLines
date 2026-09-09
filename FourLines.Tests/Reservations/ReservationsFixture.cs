using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

public class ReservationsFixture : DefaultInitializationFixture, IAsyncLifetime
{
    private bool _isGoalKeeperReservationCreated = false;
    private bool _isNoGoalKeeperReservationCreated = false;
    public Result<ConfirmReservationResponseDTO> GoalKeeperReservationResult = default!;
    public Result<ConfirmReservationResponseDTO> NoGoalKeeperReservationResult = default!;

    public ReservationsFixture() : base()
    { }

    public async Task DisposeAsync()
    {
        await DeleteDatabaseAsync();
    }

    public async Task InitializeAsync()
    {
        await DefaultSeedAsync();
        await SeedLocalTestingDataAsync();
    }

    public override async Task SeedLocalTestingDataAsync()
    {
        await using var context = CreateContext();
        await DbOperations.CreateRecord<Sport>(TestDataSource.Sport2, context);

        await DbOperations.CreateRecord<User>(TestDataSource.UserPlayer2, context);

        await DbOperations.CreateRecord<Facility>(TestDataSource.Facility2, context);
        await DbOperations.CreateRecord<Facility>(TestDataSource.Facility3, context);

        await DbOperations.CreateRecord<Court>(TestDataSource.Court3, context);
        await DbOperations.CreateRecord<Court>(TestDataSource.Court4, context);

        await DbOperations.CreateRecord<FacilitySchedule>(TestDataSource.FacilitySchedule1, context);
        await DbOperations.CreateRecord<FacilitySchedule>(TestDataSource.FacilitySchedule2, context);
        await DbOperations.CreateRecord<FacilitySchedule>(TestDataSource.FacilitySchedule4, context);

        await DbOperations.CreateRecord<Reservation>(TestDataSource.DefaultReservation, context);
        await DbOperations.CreateRecord<Reservation>(TestDataSource.Reservation2, context);

        await EnsureGoalKeeperReservationCreatedAsync();
        await EnsureNoGoalKeeperReservationCreatedAsync();

    }

    public async Task EnsureGoalKeeperReservationCreatedAsync()
    {
        if (!_isGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            GoalKeeperReservationResult = await reservationHandler.Create(TestDataSource.CreateGoalKeeperReservationTest);

            _isGoalKeeperReservationCreated = true;
        }
    }

    public async Task EnsureNoGoalKeeperReservationCreatedAsync()
    {
        if (!_isNoGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                ServiceProvider.GetRequiredService<IReservationHandler>();
            NoGoalKeeperReservationResult = await reservationHandler.Create(TestDataSource.CreateNoGoalKeeperReservationTest);

            _isNoGoalKeeperReservationCreated = true;
        }
    }

}
