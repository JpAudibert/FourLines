namespace FourLines.Api.Models.Users.Request;

public class LoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = default!;
}
