namespace FourLines.Tests.Reservations;

[CollectionDefinition(Name)]
public class ReservationsCollection : ICollectionFixture<ReservationsFixture>
{
    public const string Name = "Reservations";
}