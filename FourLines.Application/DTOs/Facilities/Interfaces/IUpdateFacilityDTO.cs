namespace FourLines.Application.DTOs.Facilities.Interfaces
{
    public interface IUpdateFacilityDTO
    {
        string Address { get; init; }
        string City { get; init; }
        Guid Id { get; init; }
        string Name { get; init; }
        Guid OwnerId { get; init; }
        string RegistrationNumber { get; init; }
        string State { get; init; }
        string ZipCode { get; init; }
    }
}