using FourLines.Application.DTOs.Facilities.Interfaces;

namespace FourLines.Application.Interfaces;

public interface IFacilityHandler
    : ICrudHandler<Facility, ICreateFacilityDTO, IUpdateFacilityDTO, IDeleteFacilityDTO>
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
