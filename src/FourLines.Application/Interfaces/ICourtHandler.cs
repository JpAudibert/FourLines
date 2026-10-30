using FourLines.Application.DTOs.Courts;

namespace FourLines.Application.Interfaces;

public interface ICourtHandler : ICrudHandler<Court, CreateCourtDTO, UpdateCourtDTO, DeleteCourtDTO>
{
    Task<Result<Court>> GetCourtFromFacility(Guid ownerId, bool isAdmin, Guid facilityId, Guid courtId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Court>>> GetAllCourtsFromFacility(
        Guid facilityId,
        CancellationToken cancellationToken = default
    );
}
