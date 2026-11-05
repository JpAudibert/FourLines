namespace FourLines.Api.Models.Presentation.Response;

/// <summary>A player participating in a match.</summary>
public sealed record PlayerResponse(
    Guid UserId,
    string UserName,
    bool IsFixedPosition,
    int TeamNumber
);
