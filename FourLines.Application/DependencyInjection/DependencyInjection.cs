using FourLines.Application.Strategies;

namespace FourLines.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ITokenProvider, JwtTokenProvider>();

        services.AddSingleton<PasswordHasher<User>>();

        services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();
        services.AddScoped<AuthenticationHandler>();

        services.AddScoped<UserHandler>();
        services.AddScoped<IFacilityHandler, FacilityHandler>();
        services.AddScoped<IFacilityScheduleHandler, FacilityScheduleHandler>();
        services.AddScoped<ICourtHandler, CourtHandler>();

        if (configuration.GetValue<bool>("UseInMemory", false))
        {
            services.AddScoped<ICourtLockStrategies, SqliteCourtLockStrategy>();
        }
        else
        {
            services.AddScoped<ICourtLockStrategies, PostgresCourtLockStrategy>();
        }

        services.AddScoped<IReservationValidator, ReservationValidator>();
        services.AddScoped<IReservationHandler, ReservationHandler>();
        services.AddScoped<IMatchHandler, MatchHandler>();

        services.AddScoped<SeederHandler>();

        return services;
    }
}
