namespace FourLines.Domain.Models;

public record Court : BaseEntity
{
    public Guid FacilityId { get; init; }
    public Guid SportId { get; init; }
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
    public Money DefaultPrice { get; init; } = default!;
    public int RentingPeriodInMinutes { get; init; }
    public int MaintenancePeriodInMinutes { get; init; }

    public Sport Sport { get; init; } = default!;
    public Facility Facility { get; init; } = default!;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
