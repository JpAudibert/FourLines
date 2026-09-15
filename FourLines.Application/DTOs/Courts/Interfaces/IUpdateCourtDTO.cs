namespace FourLines.Application.DTOs.Courts.Interfaces;

public interface IUpdateCourtDTO
{
    Guid FacilityId { get; init; }
    Guid Id { get; init; }
    bool IsActive { get; init; }
    Money DefaultPrice { get; init; }
    int RentingPeriodInMinutes { get; init; }
    int MaintenancePeriodInMinutes { get; init; }
    
    string Name { get; init; }
    Guid SportId { get; init; }
}