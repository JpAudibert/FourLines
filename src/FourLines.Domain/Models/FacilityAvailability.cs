namespace FourLines.Domain.Models;

public record FacilityAvailability
{
    public Guid CourtId { get; init; }
    public DayOfWeek DayOfWeek { get; init; }
    public int RentingPeriodInMinutes { get; init; }
    public int MaintenancePeriodInMinutes { get; init; }
    public decimal PriceAmount { get; init; }
    public string PriceCurrency { get; init; } = default!;
    public string TimeZoneId { get; init; } = default!;
    public TimeOnly OpensAt { get; init; }
    public TimeOnly ClosesAt { get; init; }
}
