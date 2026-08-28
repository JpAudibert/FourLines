namespace FourLines.Application.DTOs.Courts;

public class DeleteCourtDTO
{
    public Guid OwnerId { get; set; }
    public Guid FacilityId { get; set; }
    public Guid CourtId { get; set; }
}
