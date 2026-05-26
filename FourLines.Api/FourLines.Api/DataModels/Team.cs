using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Table("Teams")]
public class Team
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MatchId { get; set; }

    [ForeignKey(nameof(MatchId))]
    public Match Match { get; set; } = default!;

    [Required]
    public string Name { get; set; } = default!;

}
