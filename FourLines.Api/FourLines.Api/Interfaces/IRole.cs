using FourLines.Api.DataModels;

namespace FourLines.Api.Interfaces;

public interface IRole
{
    int Id { get; set; }
    string Name { get; set; }
    IEnumerable<User> Users { get; set; }
}
