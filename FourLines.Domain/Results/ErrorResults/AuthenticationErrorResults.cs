namespace FourLines.Domain.Results.ErrorResults;

public static class AuthenticationErrorResults
{
    public static readonly Error UnknownUser = new("Authentication.UnknownUser", "User not registered");
    public static readonly Error InvalidPassword = new("Authentication.InvalidPassword", "Invalid password");
}
