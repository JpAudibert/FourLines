namespace FourLines.Domain.Models;

public record Match : BaseEntity
{
    public const int MATCH_CODE_LENGTH = 6;
    public Guid ReservationId { get; init; }
    public Guid SportId { get; init; }
    public string Code { get; init; } = default!;
    public string? Name { get; set; }

    public Reservation Reservation { get; init; } = default!;
    public Sport Sport { get; init; } = default!;

    public ICollection<MatchesUsers> MatchesUsers = new List<MatchesUsers>() { };

    public static string GenerateMatchCode()
    {
        double codeSize = Math.Pow(10, MATCH_CODE_LENGTH);

        return Random.Shared.Next(0, (int)codeSize).ToString("D6");
    }
}
