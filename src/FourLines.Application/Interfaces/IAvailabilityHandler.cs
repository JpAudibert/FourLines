namespace FourLines.Application.Interfaces;

public interface IAvailabilityHandler
{
    Task<FacilityTimeSlot> GetTimeSlots(
        Guid courtId,
        DayOfWeek selectedDayOfTheWeek,
        CancellationToken cancellationToken = default
    );
}
