namespace FourLines.Domain.Results.ErrorResults;

public static class ReservationsErrorResults
{
    public static readonly Error CreationUnknownCourt = new("ReservationsCreation.UnknownCourt", "No court found for the specified court id.");
    public static readonly Error CreationUnknownUser = new("ReservationsCreation.UnknownUser", "No user found for the specified user id.");

    public static readonly Error DeletionReservationDoesNotExist = new("ReservationsDeletion.ReservationDoesNotExist", "No reservation found for the specified reservation id and user id.");

    public static readonly Error GetAllNoReservationsForUser = new("GetAllForUser.NoReservationsForUser", "No user found for the specified user id.");
   
    public static readonly Error GetAllNoReservationsForCourt = new("GetAllForCourt.NoReservationsForCourt", "No user found for the specified court id.");

    public static readonly Error GetOneReservationDoesNotExist = new("ReservationsGetOne.UnknownReservation", "No reservation found for the specified reservation id and user id.");
}
