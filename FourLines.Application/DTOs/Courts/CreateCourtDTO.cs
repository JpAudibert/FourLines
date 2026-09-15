using FourLines.Application.DTOs.Courts.Interfaces;

namespace FourLines.Application.DTOs.Courts;

public record CreateCourtDTO : ICreateCourtDTO
{
    public Guid OwnerId { get; init; }
    public Guid FacilityId { get; init; }
    public Guid SportId { get; init; }
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
    public Money DefaultPrice { get; init; } = default!;
    public int RentingPeriodInMinutes { get; init; }
    public int MaintenancePeriodInMinutes { get; init; }
}
