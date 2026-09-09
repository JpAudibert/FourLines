using FourLines.Application.DTOs.Courts.Interfaces;

namespace FourLines.Application.DTOs.Courts;

public record DeleteCourtDTO : IDeleteCourtDTO
{
    public Guid FacilityId { get; init; }
    public Guid CourtId { get; init; }
}
