using FourLines.Application.DTOs.FacilitySchedules.Interfaces;

namespace FourLines.Application.DTOs.FacilitySchedules;

public record DeleteFacilityScheduleDTO : IDeleteFacilityScheduleDTO
{
    public Guid FacilityId { get; init; }
    public Guid ScheduleId { get; init; }
}
