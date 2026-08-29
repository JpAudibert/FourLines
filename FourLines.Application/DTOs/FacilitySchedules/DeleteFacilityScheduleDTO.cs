namespace FourLines.Application.DTOs.FacilitySchedules;

public class DeleteFacilityScheduleDTO
{
    public Guid OwnerId { get; set; }
    public Guid FacilityId { get; set; }
    public Guid ScheduleId { get; set; }
}
