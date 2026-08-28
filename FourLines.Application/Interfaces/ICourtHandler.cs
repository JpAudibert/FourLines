namespace FourLines.Application.Interfaces;

public interface ICourtHandler : ICrudHandler<Court, CreateCourtDTO, UpdateCourtDTO, DeleteCourtDTO>
{
    Task<Result<Court>> GetFacility(Guid ownerId, Guid facilityId, Guid courtId);
    Task<Result<IEnumerable<Court>>> GetAllCourtsFromFacility(Guid ownerId, Guid facilityId);
}
