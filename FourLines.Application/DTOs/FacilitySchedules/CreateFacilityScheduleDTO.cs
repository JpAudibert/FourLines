using FourLines.Application.DTOs.FacilitySchedules.Interfaces;

namespace FourLines.Application.DTOs.FacilitySchedules;

public record CreateFacilityScheduleDTO : ICreateFacilityScheduleDTO
{
    public Guid FacilityId { get; init; }
    public DayOfWeek DayOfWeek { get; init; }
    public TimeOnly OpensAt { get; init; }
    public TimeOnly ClosesAt { get; init; }
}
