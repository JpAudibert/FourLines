using FourLines.Application.DTOs.Facilities;

namespace FourLines.Application.Interfaces;

public interface IFacilityHandler
    : ICrudHandler<Facility, CreateFacilityDTO, UpdateFacilityDTO, DeleteFacilityDTO>
{
    Task<Result<IEnumerable<Facility>>> GetFacilitiesFromOwner(
        Guid ownerId,
        CancellationToken cancellationToken = default
    );
    Task<Result<Facility>> GetFacilityFromOwner(
        Guid ownerId,
        Guid facilityId,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<Facility>>> GetAllFacilities(
        CancellationToken cancellationToken = default
    );
}
