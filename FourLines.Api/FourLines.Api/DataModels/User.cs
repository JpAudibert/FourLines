using FourLines.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Index(nameof(Slug), IsUnique = true)]
[Table("Users")]
public class User : IUser
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Email { get; set; } = default!;

    [Required]
    public string PasswordHash { get; set; } = default!;

    [Required]
    public string Slug { get; set; } = default!;

    [Required]
    public DateTime Birthday { get; set; }

    [Required]
    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public IRole Role { get; set; } = default!;
}
