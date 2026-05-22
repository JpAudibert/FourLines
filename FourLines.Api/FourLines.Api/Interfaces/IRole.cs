namespace FourLines.Api.Interfaces;

public interface IRole
{
    int Id { get; set; }
    string Name { get; set; }
    IEnumerable<IUser> Users { get; set; }
}
