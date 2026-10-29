namespace FourLines.Domain.Results.ErrorResults;

public class MatchesErrorResults
{
    public static readonly Error MatchNotFound = new("Match.MatchNotFound", "The specified match does not exist.");

    public static readonly Error IngressMatchNotFound = new("Ingress.IngressMatchNotFound", "The specified match does not exist.");
    public static readonly Error IngressUserNotFound = new("Ingress.IngressUserNotFound", "The specified user does not exist.");

    public static readonly Error IngressSportDoesNotHaveFixedGoalKeeper = new("Ingress.IngressSportDoesNotHaveFixedGoalKeeper", "The specified sport does not have a fixed goal keeper.");

    public static readonly Error LeaveMatchNotFound = new("Match.LeaveMatchNotFound", "The specified match entry does not exist.");
}
