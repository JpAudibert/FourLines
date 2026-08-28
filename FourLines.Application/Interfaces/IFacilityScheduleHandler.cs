namespace FourLines.Application.Interfaces;

public interface IFacilityScheduleHandler 
    : ICrudHandler<FacilitySchedule, CreateFacilityScheduleDTO, UpdateFacilityScheduleDTO, DeleteFacilityScheduleDTO>
{
    Task<Result<IEnumerable<FacilitySchedule>>> CreateMultiple(List<CreateFacilityScheduleDTO> newSchedules);
    Task<Result<IEnumerable<FacilitySchedule>>> GetSchedules(Guid ownerId, Guid facilityId);
}
