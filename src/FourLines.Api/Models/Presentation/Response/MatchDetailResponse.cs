namespace FourLines.Api.Models.Presentation.Response;

/// <summary>Detailed match information.</summary>
public sealed record MatchDetailResponse(
    Guid MatchId,
    Guid CourtId,
    string Name,
    Guid SportId,
    string SportName,
    string CourtName,
    string FacilityName,
    IReadOnlyList<PlayerResponse> Players
);
