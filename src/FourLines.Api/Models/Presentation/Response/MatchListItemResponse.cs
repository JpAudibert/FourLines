namespace FourLines.Api.Models.Presentation.Response;

/// <summary>A match item shown in a player's match list.</summary>
public sealed record MatchListItemResponse(
    Guid MatchId,
    string Name,
    Guid SportId,
    string SportName,
    string CourtName,
    string FacilityName,
    IReadOnlyList<PlayerResponse> Players
);
