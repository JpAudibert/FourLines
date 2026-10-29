namespace FourLines.Application.DTOs;

public record AuthenticationDTO
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}
