namespace FourLines.Domain.Models;

public class Role : BaseEntity
{
    public string Name { get; init; } = default!;

    public IEnumerable<User> Users { get; init; } = [];

}
