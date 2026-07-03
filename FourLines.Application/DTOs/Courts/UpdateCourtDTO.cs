namespace FourLines.Application.DTOs.Courts;

public class UpdateCourtDTO
{
    public Guid Id { get; init; } = default!;
    public Guid OwnerId { get; init; } = default!;
    public Guid FacilityId { get; init; } = default!;
    public Guid SportId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
}
