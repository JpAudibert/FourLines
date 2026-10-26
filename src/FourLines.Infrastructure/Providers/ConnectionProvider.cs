using Npgsql;

namespace FourLines.Infrastructure.Providers;

public class ConnectionProvider(
    IConfiguration configuration,
    string defaultConnectionString = default!
) : IDisposable
{
    public NpgsqlConnection Provide()
    {
        if (!string.IsNullOrEmpty(defaultConnectionString))
            return new NpgsqlConnection(defaultConnectionString);

        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Postgres")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        return new NpgsqlConnection(connectionString);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
