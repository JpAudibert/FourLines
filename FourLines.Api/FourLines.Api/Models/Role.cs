using FourLines.Api.Interfaces;

namespace FourLines.Api.Models;

public class Role : IRole
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public IEnumerable<IUser> Users { get; set; } = [];
}
