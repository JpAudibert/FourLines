namespace FourLines.Api.Models;

public class Facility : BaseEntity
{
    public Guid OwnerId { get; init; }
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string City { get; init; } = default!;
    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;

    public User Owner { get; init; } = default!;
    public IEnumerable<Court> Courts { get; init; } = [];
}
