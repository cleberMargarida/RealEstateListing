namespace RealEstateListing.IntegrationTests.Infrastructure;

/// <summary>
/// xUnit collection definition that ensures the SQL Server container is started
/// before any tests in this collection run.
/// The SqlServerContainerFixture is shared across all test classes in this collection.
/// </summary>
[CollectionDefinition(nameof(IntegrationTestsCollection))]
public class IntegrationTestsCollection
    : ICollectionFixture<SqlServerContainerFixture>
    , ICollectionFixture<ApiFixture>
{
    // This class has no code, and is never created.
    // Its purpose is simply to be the place to apply [CollectionDefinition]
    // and all the ICollectionFixture<> interfaces.
}
