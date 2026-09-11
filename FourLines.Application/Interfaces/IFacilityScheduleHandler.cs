using FourLines.Application.DTOs.FacilitySchedules.Interfaces;

namespace FourLines.Application.Interfaces;

public interface IFacilityScheduleHandler 
    : ICrudHandler<FacilitySchedule, ICreateFacilityScheduleDTO, IUpdateFacilityScheduleDTO, IDeleteFacilityScheduleDTO>
{
    Task<Result<IEnumerable<FacilitySchedule>>> CreateMultiple(List<ICreateFacilityScheduleDTO> newSchedules);
    Task<Result<IEnumerable<FacilitySchedule>>> GetSchedules(Guid facilityId);
}
