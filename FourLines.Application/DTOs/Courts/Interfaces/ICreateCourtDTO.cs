namespace FourLines.Application.DTOs.Courts.Interfaces;

public interface ICreateCourtDTO
{
    Guid FacilityId { get; init; }
    bool IsActive { get; init; }
    string Name { get; init; }
    Money DefaultPrice { get; init; }
    int RentingPeriodInMinutes { get; init; }
    int MaintenancePeriodInMinutes { get; init; }

    Guid OwnerId { get; init; }
    Guid SportId { get; init; }
}