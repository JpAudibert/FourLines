

namespace FourLines.Api.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = default!;
}
