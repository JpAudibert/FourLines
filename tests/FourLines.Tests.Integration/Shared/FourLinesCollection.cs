namespace FourLines.Tests.Integration.Shared;

[CollectionDefinition(Name)]
public class FourLinesCollection : ICollectionFixture<FourLinesFixture>
{
    public const string Name = "FourLinesCollection";
}
