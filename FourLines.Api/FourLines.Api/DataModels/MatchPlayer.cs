using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Table("match_players")]
public class MatchPlayer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MatchId { get; set; }

    [ForeignKey(nameof(MatchId))]
    public Match Match { get; set; } = default!;

    [Required]
    public int PlayerId { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public User User { get; set; } = default!;

    [Required]
    public bool IsGoalKeeper { get; set; } = default!;

}
