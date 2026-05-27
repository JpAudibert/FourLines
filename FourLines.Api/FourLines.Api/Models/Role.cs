namespace FourLines.Api.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public IEnumerable<User> Users { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
