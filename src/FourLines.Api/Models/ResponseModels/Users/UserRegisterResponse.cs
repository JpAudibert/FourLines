namespace FourLines.Api.Models.ResponseModels.Users;

public record UserRegisterResponse
{
    public Guid Id { get; init; }
    public Guid RoleId { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
}
