namespace FourLines.Application.Authentication.DTOs;

public record LoginRequest(
    string Email,
    string Password
);
