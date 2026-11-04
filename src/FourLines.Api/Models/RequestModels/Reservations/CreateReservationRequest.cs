namespace FourLines.Api.Models.RequestModels.Reservations;

public class CreateReservationRequest
{
    [Required(ErrorMessage = "Court ID is required.")]
    public required Guid CourtId { get; init; }

    [Required(ErrorMessage = "Reservation period is required.")]
    public required TimeRange Period { get; init; }

    [Required(ErrorMessage = "Reservation status is required.")]
    public required ReservationStatus Status { get; init; }

    [Required(ErrorMessage = "Reservation price is required.")]
    public required ReservationPriceRequest Price { get; init; }
}

public class ReservationPriceRequest
{
    [Required(ErrorMessage = "Reservation price amount is required.")]
    public required decimal Amount { get; init; }

    [Required(ErrorMessage = "Reservation price currency is required.")]
    [StringLength(
        3,
        MinimumLength = 3,
        ErrorMessage = "Reservation price currency must be 3 characters."
    )]
    public required string Currency { get; init; }
}
