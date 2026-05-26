using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Table("Matches")]
public class Match
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ReservationId { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    [Required]
    public int MaxPlayers { get; set; } = default!;

    [Required]
    public bool IsGoalKeeper { get; set; } = default!;

    [Required]
    public int Status { get; set; } = default!;

    [ForeignKey(nameof(ReservationId))]
    public Reservation Reservation { get; set; } = default!;

}
