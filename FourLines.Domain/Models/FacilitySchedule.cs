namespace FourLines.Domain.Models;

public class FacilitySchedule : BaseEntity
{
    public Guid FacilityId { get; init; }
    public DayOfWeek DayOfWeek { get; init; }
    public TimeOnly OpensAt { get; init; }
    public TimeOnly ClosesAt { get; init; }

    public Facility Facility { get; init; } = default!;
}
