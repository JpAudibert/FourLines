using FourLines.Application.DTOs.Facilities.Interfaces;

namespace FourLines.Application.DTOs.Facilities;

public record DeleteFacilityDTO : IDeleteFacilityDTO
{
    public Guid OwnerId { get; init; }
    public Guid FacilityId { get; init; }
}
