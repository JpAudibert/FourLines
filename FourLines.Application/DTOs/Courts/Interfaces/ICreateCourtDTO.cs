namespace FourLines.Application.DTOs.Courts.Interfaces;

public interface ICreateCourtDTO
{
    Guid FacilityId { get; init; }
    bool IsActive { get; init; }
    string Name { get; init; }
    Guid OwnerId { get; init; }
    Guid SportId { get; init; }
}