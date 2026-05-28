namespace FourLines.Api.Models;

public class Court : BaseEntity
{
    public int FacilityId { get; set; }
    public int SportId { get; set; }
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Sport Sport { get; init; } = default!;
    public Facility Facility { get; init; } = default!;
}
