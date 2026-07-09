namespace FourLines.Domain.Results.ErrorResults;

public static class ReservationsErrorResults
{
    public static readonly Error CreationUnknownCourt = new("ReservationsCreation.UnknownCourt", "No court found for the specified court id.");
    public static readonly Error CreationUnknownUser = new("ReservationsCreation.UnknownUser", "No user found for the specified user id.");
    public static readonly Error CreationOutsideFacilitySchedule = new("ReservationsCreation.OutsideFacilitySchedule", "The reservation period is outside the facility schedule.");
    public static readonly Error CreationOverlappingReservation = new("ReservationsCreation.OverlappingReservation", "The reservation period overlaps with an existing reservation for the same court.");

    public static readonly Error CreationInvalidDates = new("ReservationsCreation.InvalidDates", "The reservation period is invalid.");
    public static readonly Error CreationInvalidDuration = new("ReservationsCreation.InvalidDuration ", "The reservation duration is invalid or less than or equal to 0.");
    public static readonly Error CreationStartAndEndInThePast = new("ReservationsCreation.StartAndEndInThePast", "The reservation period is in the past.");
    public static readonly Error CreationStartAndEndNotInTheSameDay = new("ReservationsCreation.StartAndEndNotInTheSameDay", "The reservation period must start and end on the same day.");
    public static readonly Error CreationDurationTimeDifferentThanConfiguration = new("ReservationsCreation.DurationTimeDifferentThanConfiguration", "The reservation duration must be equal to the configured duration.");

    public static readonly Error UpdateReservationDoesNotExist = new("ReservationsUpdateStatus.ReservationDoesNotExist", "No reservation found for the specified reservation id and user id.");

    public static readonly Error DeletionReservationDoesNotExist = new("ReservationsDeletion.ReservationDoesNotExist", "No reservation found for the specified reservation id and user id.");

    public static readonly Error GetAllNoReservationsForUser = new("GetAllForUser.NoReservationsForUser", "No user found for the specified user id.");
    public static readonly Error GetAllNoReservationsForCourt = new("GetAllForCourt.NoReservationsForCourt", "No user found for the specified court id.");

    public static readonly Error GetOneReservationDoesNotExist = new("ReservationsGetOne.UnknownReservation", "No reservation found for the specified reservation id and user id.");
}
