namespace FourLines.Application.DTOs.Users;

public record UserUpdateDTO
{
    public string Name { get; init; } = default!;
    public DateOnly Birthday { get; init; }
    public string Phone { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
    public bool IsActive { get; init; } = default!;
}
