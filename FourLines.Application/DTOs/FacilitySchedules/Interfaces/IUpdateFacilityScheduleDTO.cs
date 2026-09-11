namespace FourLines.Application.DTOs.FacilitySchedules.Interfaces
{
    public interface IUpdateFacilityScheduleDTO
    {
        TimeOnly ClosesAt { get; init; }
        DayOfWeek DayOfWeek { get; init; }
        Guid FacilityId { get; init; }
        Guid Id { get; init; }
        TimeOnly OpensAt { get; init; }
    }
}