namespace FourLines.Api.ViewModels.Reservations
{
    public class CreateReservationViewModel
    {
        public Guid CourtId { get; init; }
        public TimeRange Period { get; init; } = default!;
        public ReservationStatus Status { get; init; } = default!;
    }
}
