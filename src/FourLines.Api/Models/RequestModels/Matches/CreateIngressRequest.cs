namespace FourLines.Api.Models.RequestModels.Matches;

public record CreateIngressRequest
{
    public string Code { get; init; } = default!;
}
