namespace FourLines.Api.Models;

public class User : BaseEntity
{
    public int RoleId { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string PasswordHash { get; init; } = default!;
    public DateOnly Birthday { get; init; }
    public string Phone { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
    public bool IsActive { get; init; } = default!;

    public Role Role { get; init; } = default!;
    public IEnumerable<Facility> Facilities { get; init; } = [];
}
