namespace FourLines.Api.Models.ResponseModels.Users;

public record UserReadResponse
{
    public Guid Id { get; init; }
    public Guid RoleId { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string PasswordHash { get; init; } = default!;
    public DateOnly Birthday { get; init; }
    public string Phone { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
    public bool IsActive { get; init; } = default!;
}
