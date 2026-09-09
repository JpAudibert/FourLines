namespace FourLines.Application.DTOs.Courts.Interfaces
{
    public interface IDeleteCourtDTO
    {
        Guid CourtId { get; init; }
        Guid FacilityId { get; init; }

        string ToString();
    }
}