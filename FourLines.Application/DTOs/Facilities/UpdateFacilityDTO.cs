namespace FourLines.Application.DTOs.Facilities;

public record UpdateFacilityDTO
{
    public Guid Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string City { get; init; } = default!;
    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
}
