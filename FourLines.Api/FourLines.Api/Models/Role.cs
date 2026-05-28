namespace FourLines.Api.Models;

public class Role : BaseEntity
{
    public string Name { get; set; } = default!;

    public IEnumerable<User> Users { get; set; } = [];

}
