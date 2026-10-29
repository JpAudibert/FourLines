namespace FourLines.Api.Models.Users.Request;

public record UserUpdateRequest
{
    public string Name { get; init; } = default!;
    public DateOnly Birthday { get; init; }
    public string Phone { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
    public bool IsActive { get; init; } = default!;
}
