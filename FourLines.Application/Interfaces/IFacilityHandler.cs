namespace FourLines.Application.Interfaces;

public interface IFacilityHandler : ICrudHandler<Facility, CreateFacilityDTO, UpdateFacilityDTO, DeleteFacilityDTO>
{
    Task<Result<IEnumerable<Facility>>> GetFacilitiesFromOwner(Guid ownerId);
    Task<Result<Facility>> GetFacilityFromOwner(Guid ownerId, Guid facilityId);
    Task<Result<IEnumerable<Facility>>> GetAllFacilities();
}
