namespace FourLines.Application.DTOs.Courts;

public record DeleteCourtDTO
{
    public Guid FacilityId { get; init; }
    public Guid OwnerId { get; init; }
    public bool IsAdmin { get; init; }
    public Guid CourtId { get; init; }
}
