namespace FourLines.Application.DTOs.FacilitySchedules;

public record CreateFacilityScheduleDTO
{
    public Guid OwnerId { get; init; }
    public bool IsAdmin { get; init; }
    public Guid FacilityId { get; init; }
    public DayOfWeek DayOfWeek { get; init; }
    public TimeOnly OpensAt { get; init; }
    public TimeOnly ClosesAt { get; init; }
}
