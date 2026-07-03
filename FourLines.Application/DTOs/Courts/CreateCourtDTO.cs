namespace FourLines.Application.DTOs.Courts;

public class CreateCourtDTO
{
    public Guid OwnerId { get; init; }
    public Guid FacilityId { get; init; }
    public Guid SportId { get; init; }
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
}
