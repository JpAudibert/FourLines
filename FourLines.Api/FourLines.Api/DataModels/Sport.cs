using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Table("sports")]
public class Sport
{
    [Key]
    public int Id { get; init; }

    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = default!;

    [Required]
    public bool Indoor { get; init; } 

    [Required]
    public int StartingPlayersCount { get; init; } = default!;

    [Required]
    public int MaxPlayersCount { get; init; } = default!;

    [Required]
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    [Required]
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;

    public IEnumerable<Court> Courts { get; init; } = [];
}
