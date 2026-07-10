namespace FourLines.Domain.Models;

public class User : BaseEntity
{
    public Guid RoleId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public DateOnly Birthday { get; set; }
    public string Phone { get; set; } = default!;
    public string RegistrationNumber { get; set; } = default!;
    public bool IsActive { get; set; } = default!;

    public Role Role { get; set; } = default!;
    public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
