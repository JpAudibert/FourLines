namespace FourLines.Api.Models.Presentation.Response;

/// <summary>Facilities returned for the home response.</summary>
public sealed record HomeResponse(
    IReadOnlyList<FacilityListItemResponse> Facilities
);
