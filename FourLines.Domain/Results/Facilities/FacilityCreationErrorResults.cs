namespace FourLines.Domain.Results.Facilities;

public class FacilityCreationErrorResults
{
    public static readonly Error OwnerDoesNotExists = new("FacilityCreation.OwnerDoesNotExists", "The specified owner does not exist.");
}
