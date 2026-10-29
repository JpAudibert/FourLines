using FourLines.Application.DTOs.Facilities;

namespace FourLines.Application.Interfaces;

public interface IFacilityHandler
    : ICrudHandler<Facility, CreateFacilityDTO, UpdateFacilityDTO, DeleteFacilityDTO>
{
    Task<Result<IEnumerable<Facility>>> GetFacilitiesFromOwner(
        Guid ownerId,
        CancellationToken cancellationToken = default,
        bool isAdmin = false
    );
    Task<Result<Facility>> GetFacilityFromOwner(
        Guid ownerId,
        Guid facilityId,
        CancellationToken cancellationToken = default,
        bool isAdmin = false
    );
    Task<Result<IEnumerable<Facility>>> GetAllFacilities(
        Guid ownerId,
        bool isAdmin,
        CancellationToken cancellationToken = default
    );
}
