namespace FourLines.Domain.Models;

public record Match : BaseEntity
{
    public Guid ReservationId { get; init; }
    public Guid SportId { get; init; }
    public string Code { get; init; } = default!;
    public string? Name { get; init; }

    public Reservation Reservation { get; init; } = default!;
    public Sport Sport { get; init; } = default!;

    public ICollection<MatchesUsers> MatchesUsers = new List<MatchesUsers>() { };
}
