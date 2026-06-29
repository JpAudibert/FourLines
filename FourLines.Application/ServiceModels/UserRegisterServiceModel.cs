using FourLines.Domain.Constants;

namespace FourLines.Application.ServiceModels;

public record UserRegisterServiceModel
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public DateOnly Birthday { get; init; }
    public string Phone { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
    public string RoleName { get; init; } = RoleConstants.Player;
}
