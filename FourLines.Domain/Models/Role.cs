namespace FourLines.Domain.Models;

public class Role : BaseEntity
{
    public string Name { get; init; } = default!;

    public ICollection<User> Users { get; set; } = new List<User>();

}
