using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Table("facilities")]
public class Facility
{
    [Key]
    public int Id { get; init; }

    [Required]
    public int OwnerId { get; init; }

    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = default!;

    [Required]
    [MaxLength(200)]
    public string Address { get; init; } = default!;

    [Required]
    [MaxLength(100)]
    public string City { get; init; } = default!;

    [Required]
    [MaxLength(100)]
    public string State { get; init; } = default!;

    [Required]
    [MaxLength(20)]
    public string ZipCode { get; init; } = default!;

    [Required]
    [MaxLength(20)]
    public string RegistrationNumber { get; init; } = default!;

    [Required]
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    [Required]
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;

    [ForeignKey(nameof(OwnerId))]
    public User Owner { get; init; } = default!;

    public IEnumerable<Court> Courts { get; init; } = [];
}
