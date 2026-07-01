namespace FourLines.Application.DTOs;

public record UserRegisterDTO
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateOnly Birthday { get; init; }
    public string Phone { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
    public Guid RoleId { get; init; } = default!;
    public bool IsActive { get; init; } = default!;
}
