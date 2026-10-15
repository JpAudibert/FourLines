namespace FourLines.Application.Interfaces;

public interface IAvailabilityHandler
{
    IAsyncEnumerable<TimeSlot> GetTimeSlots(
        Guid courtId,
        DayOfWeek selectedDayOfTheWeek,
        CancellationToken cancellationToken = default
    );
}
