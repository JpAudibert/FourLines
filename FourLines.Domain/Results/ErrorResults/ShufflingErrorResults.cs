namespace FourLines.Domain.Results.ErrorResults;

public static class ShufflingErrorResults
{
    public static readonly Error ShufflingUnknownMatch = new("Shuffling.ShufflingUnknownMatch", "No match found to have its players shuffled.");
}