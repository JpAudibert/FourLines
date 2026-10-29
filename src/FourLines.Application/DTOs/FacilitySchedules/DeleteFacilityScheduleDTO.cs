namespace FourLines.Application.DTOs.FacilitySchedules;

public record DeleteFacilityScheduleDTO
{
    public Guid OwnerId { get; init; }
    public bool IsAdmin { get; init; }
    public Guid FacilityId { get; init; }
    public Guid ScheduleId { get; init; }
}
