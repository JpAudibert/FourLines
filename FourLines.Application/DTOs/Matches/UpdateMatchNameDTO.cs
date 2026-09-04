namespace FourLines.Application.DTOs.Matches;

public record UpdateMatchNameDTO
{
    public Guid MatchId { get; set; }
    public string NewName { get; set; } = default!;
}
