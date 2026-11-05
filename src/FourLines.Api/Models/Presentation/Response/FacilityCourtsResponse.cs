namespace FourLines.Api.Models.Presentation.Response;

/// <summary>Facility details and its courts.</summary>
public sealed record FacilityCourtsResponse(
    Guid FacilityId,
    string FacilityName,
    string FacilityAddress,
    IReadOnlyList<CourtListItemResponse> Courts
);
