using FourLines.Application.DTOs.Courts.Interfaces;

namespace FourLines.Application.Interfaces;

public interface ICourtHandler : ICrudHandler<Court, ICreateCourtDTO, IUpdateCourtDTO, IDeleteCourtDTO>
{
    Task<Result<Court>> GetCourtFromFacility(Guid facilityId, Guid courtId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Court>>> GetAllCourtsFromFacility(Guid facilityId, CancellationToken cancellationToken = default);
}
