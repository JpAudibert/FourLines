namespace FourLines.Domain.Models;

public class Court : BaseEntity
{
    public Guid FacilityId { get; init; }
    public Guid SportId { get; init; }
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }

    public Sport Sport { get; init; } = default!;
    public Facility Facility { get; init; } = default!;
}
