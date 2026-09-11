namespace FourLines.Application.DTOs.FacilitySchedules.Interfaces
{
    public interface ICreateFacilityScheduleDTO
    {
        TimeOnly ClosesAt { get; init; }
        DayOfWeek DayOfWeek { get; init; }
        Guid FacilityId { get; init; }
        TimeOnly OpensAt { get; init; }
    }
}