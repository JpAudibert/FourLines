namespace FourLines.Application.DTOs.FacilitySchedules;

public record DeleteFacilityScheduleDTO
{
    public Guid FacilityId { get; init; }
    public Guid ScheduleId { get; init; }
}
