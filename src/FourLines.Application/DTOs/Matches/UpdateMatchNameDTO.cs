namespace FourLines.Application.DTOs.Matches;

public record UpdateMatchNameDTO
{
    public Guid MatchId { get; set; }
    public Guid UserId { get; init; }
    public bool IsAdmin { get; init; }
    public string NewName { get; set; } = default!;
}
