using FourLines.Api.Interfaces;

namespace FourLines.Api.Models;

public class User : IUser
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public DateTime Birthday { get; set; }
    public int RoleId { get; set; }
    public IRole Role { get; set; } = default!;
}
