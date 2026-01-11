using Testcontainers.MsSql;

namespace RealEstateListing.IntegrationTests.Infrastructure;

/// <summary>
/// Test fixture that spins up a SQL Server container for integration testing.
/// Implements IAsyncLifetime to manage the container lifecycle.
/// Sets the connection string via environment variable for the application to consume.
/// </summary>
public class SqlServerContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container;

    /// <summary>
    /// Gets the connection string to the SQL Server container.
    /// </summary>
    public string ConnectionString => _container.GetConnectionString();

    public SqlServerContainerFixture()
    {
        _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();
    }

    /// <summary>
    /// Starts the SQL Server container and sets the connection string environment variable.
    /// </summary>
    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();

        Environment.SetEnvironmentVariable(
            "ConnectionStrings:SqlServer",
            _container.GetConnectionString());
    }

    /// <summary>
    /// Stops and disposes the SQL Server container.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:SqlServer", null);
        await _container.DisposeAsync();
    }
}
