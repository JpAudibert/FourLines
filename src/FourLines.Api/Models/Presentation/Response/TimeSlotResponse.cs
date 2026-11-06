namespace FourLines.Api.Models.Presentation.Response;

/// <summary>A time slot with its price and availability.</summary>
public sealed record TimeSlotResponse(
    TimeOnly Start,
    TimeOnly End,
    MoneyResponse Price,
    bool Available
);
