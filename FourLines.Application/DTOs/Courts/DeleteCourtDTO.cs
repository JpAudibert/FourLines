namespace FourLines.Application.DTOs.Courts;

public record DeleteCourtDTO
{
    public Guid OwnerId { get; init; }
    public Guid FacilityId { get; init; }
    public Guid CourtId { get; init; }
}
