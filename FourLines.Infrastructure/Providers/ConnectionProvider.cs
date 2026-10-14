using Npgsql;

namespace FourLines.Infrastructure.Providers;

public class ConnectionProvider(IConfiguration configuration) : IDisposable
{
    public NpgsqlConnection Provide() => new(configuration.GetConnectionString("DefaultConnection"));

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
