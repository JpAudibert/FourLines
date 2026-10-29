namespace FourLines.Application.DTOs.Facilities;

public record DeleteFacilityDTO 
{
    public Guid OwnerId { get; init; }
    public bool IsAdmin { get; init; }
    public Guid FacilityId { get; init; }
}
