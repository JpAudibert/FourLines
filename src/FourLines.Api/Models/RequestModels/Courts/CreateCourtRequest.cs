namespace FourLines.Api.Models.RequestModels.Courts;

public class CreateCourtRequest
{
    [Required(ErrorMessage = "Court name is required.")]
    public string Name { get; init; } = default!;

    [Required(ErrorMessage = "Sport ID is required.")]
    public Guid SportId { get; init; } = default!;

    public bool IsActive { get; init; } = default!;
}
