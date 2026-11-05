namespace FourLines.Api.Models.Presentation.Response;

/// <summary>Court availability for the selected playing date.</summary>
public sealed record CourtAvailabilityResponse(
    Guid CourtId,
    DateOnly PlayingDate,
    IReadOnlyList<TimeSlotResponse> TimeSlots
);
