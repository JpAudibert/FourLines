namespace FourLines.Application.DTOs.Facilities.Interfaces
{
    public interface IDeleteFacilityDTO
    {
        Guid FacilityId { get; init; }
        Guid OwnerId { get; init; }
    }
}