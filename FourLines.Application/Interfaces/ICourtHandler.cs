using FourLines.Application.DTOs.Courts.Interfaces;

namespace FourLines.Application.Interfaces;

public interface ICourtHandler : ICrudHandler<Court, ICreateCourtDTO, IUpdateCourtDTO, IDeleteCourtDTO>
{
    Task<Result<Court>> GetFacility(Guid facilityId, Guid courtId);
    Task<Result<IEnumerable<Court>>> GetAllCourtsFromFacility(Guid facilityId);
}
