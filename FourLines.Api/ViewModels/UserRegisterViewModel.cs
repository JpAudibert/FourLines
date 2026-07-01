namespace FourLines.Api.ViewModels;

public record UserRegisterViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Birthday is required.")]
    public DateOnly Birthday { get; init; }

    [Required(ErrorMessage = "Phone is required.")]
    public string Phone { get; init; } = default!;

    [Required(ErrorMessage = "Registration number is required.")]
    [RegularExpression("^\\d{3}\\.\\d{3}\\.\\d{3}-\\d{2}$", ErrorMessage = "Invalid registration number format.")]
    public string RegistrationNumber { get; init; } = default!;

    public bool IsActive { get; init; } = default!;
}
