namespace FourLines.Api.Models.RequestModels.Matches;

public record UpdateMatchNameRequest
{
    public string Name { get; init; } = default!;
}
