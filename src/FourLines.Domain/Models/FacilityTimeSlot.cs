namespace FourLines.Domain.Models;

public record FacilityTimeSlot(string TimeZoneId, IReadOnlyList<TimeSlot> TimeSlots);
