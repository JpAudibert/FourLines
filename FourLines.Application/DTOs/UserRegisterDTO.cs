namespace FourLines.Application.DTOs;

public record UserRegisterDTO
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public DateOnly Birthday { get; init; }
    public string Phone { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
    public Guid RoleId { get; init; } = default!;
    public bool IsActive { get; init; } = default!;
}
