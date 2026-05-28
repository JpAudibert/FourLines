namespace FourLines.Api.Models;

public class Facility : BaseEntity
{
    public int OwnerId { get; set; }
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;
    public string RegistrationNumber { get; set; } = default!;

    public User Owner { get; init; } = default!;
    public IEnumerable<Court> Courts { get; set; } = [];
}
