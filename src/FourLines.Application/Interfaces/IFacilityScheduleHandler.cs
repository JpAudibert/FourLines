using FourLines.Application.DTOs.FacilitySchedules;

namespace FourLines.Application.Interfaces;

public interface IFacilityScheduleHandler
    : ICrudHandler<
        FacilitySchedule,
        CreateFacilityScheduleDTO,
        UpdateFacilityScheduleDTO,
        DeleteFacilityScheduleDTO
    >
{
    Task<Result<IEnumerable<FacilitySchedule>>> CreateMultiple(
        List<CreateFacilityScheduleDTO> newSchedules,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<FacilitySchedule>>> GetSchedules(
        Guid ownerId,
        bool isAdmin,
        Guid facilityId,
        CancellationToken cancellationToken = default
    );
}
