using FourLines.Api.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourLines.Api.DataModels;

[Table("Roles")]
public class Role : IRole
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public IEnumerable<User> Users { get; set; } = [];
}
