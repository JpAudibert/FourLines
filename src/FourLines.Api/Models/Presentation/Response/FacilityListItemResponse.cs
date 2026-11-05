namespace FourLines.Api.Models.Presentation.Response;

/// <summary>A facility, its available sports, and today's available time slots.</summary>
public sealed record FacilityListItemResponse(
    Guid FacilityId,
    string Name,
    string Address,
    IReadOnlyList<SportResponse> Sports,
    int AvailableTimeSlotsCount
);
