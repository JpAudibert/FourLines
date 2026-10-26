namespace FourLines.Tests.Shared;

[CollectionDefinition(Name)]
public class FourLinesCollection : ICollectionFixture<FourLinesFixture>
{
    public const string Name = "FourLinesCollection";
}
