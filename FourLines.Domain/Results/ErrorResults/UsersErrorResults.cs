namespace FourLines.Domain.Results.ErrorResults;

public static class UsersErrorResults
{
    public static readonly Error EmailAlreadyExists = new("UserCreation.EmailAlreadyExists", "Email already exists");
    public static readonly Error InvalidRole = new("UserCreation.InvalidRole", "Invalid role");
}
