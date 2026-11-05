namespace FourLines.Api.Models.Presentation.Response;

/// <summary>Money and its currency code.</summary>
public sealed record MoneyResponse(decimal Value, string Currency);
