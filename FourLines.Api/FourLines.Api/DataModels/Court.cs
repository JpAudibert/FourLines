using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Table("courts")]
public class Court
{
    [Key]
    public int Id { get; init; }

    [Required]
    public int FacilityId { get; init; }

    [Required]
    public int SportId { get; init; } = default!;

    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = default!;

    [Required]
    public bool IsActive { get; init; } = default!;

    [Required]
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    [Required]
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;

    [ForeignKey(nameof(SportId))]
    public Sport Sport { get; init; } = default!;

    [ForeignKey(nameof(FacilityId))]
    public Facility Facility { get; init; } = default!;

}
