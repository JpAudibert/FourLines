namespace FourLines.Domain.Results.User;

public static class UserCreationErrorResults
{
    public static readonly Error EmailAlreadyExists = new("UserCreation.EmailAlreadyExists", "Email already exists");
    public static readonly Error InvalidRole = new("UserCreation.InvalidRole", "Invalid role");
}
