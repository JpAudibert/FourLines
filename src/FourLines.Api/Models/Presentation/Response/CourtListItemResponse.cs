namespace FourLines.Api.Models.Presentation.Response;

/// <summary>A court offered by a facility.</summary>
public sealed record CourtListItemResponse(
    Guid CourtId,
    string Name,
    Guid SportId,
    string SportName,
    int RentingPeriodInMinutes,
    MoneyResponse DefaultPrice
);
