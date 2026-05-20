namespace FourLines.Api.Interfaces;

public interface IUser
{
    DateTime Birthday { get; set; }
    string Email { get; set; }
    int Id { get; set; }
    string Name { get; set; }
    string PasswordHash { get; set; }
    IRole Role { get; set; }
    int RoleId { get; set; }
    string Slug { get; set; }
}
