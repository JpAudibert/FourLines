using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Xml.Linq;

namespace FourLines.Api.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = default!;
}
