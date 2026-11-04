namespace FourLines.Api.Models.RequestModels.Courts;

public class UpdateCourtRequest
{
    [Required(ErrorMessage = "Court name is required.")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Sport ID is required.")]
    public required Guid SportId { get; init; }

    [Required(ErrorMessage = "Court active status is required.")]
    public required bool IsActive { get; init; }

    [Required(ErrorMessage = "Court default price is required.")]
    public required Money DefaultPrice { get; init; }

    [Required(ErrorMessage = "Court renting period is required.")]
    public required int RentingPeriodInMinutes { get; init; }

    [Required(ErrorMessage = "Court maintenance period is required.")]
    public required int MaintenancePeriodInMinutes { get; init; }
}
