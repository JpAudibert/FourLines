namespace FourLines.Application.DTOs.FacilitySchedules.Interfaces
{
    public interface IDeleteFacilityScheduleDTO
    {
        Guid FacilityId { get; init; }
        Guid ScheduleId { get; init; }
    }
}