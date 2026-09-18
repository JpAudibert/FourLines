using FourLines.Application.Strategies;

namespace FourLines.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITokenProvider, JwtTokenProvider>();

        services.AddSingleton<PasswordHasher<User>>();

        services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();
        services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();

        services.AddScoped<IUserHandler, UserHandler>();
        services.AddScoped<IFacilityHandler, FacilityHandler>();
        services.AddScoped<IFacilityScheduleHandler, FacilityScheduleHandler>();
        services.AddScoped<ICourtHandler, CourtHandler>();

        services.AddScoped<ICourtLockStrategies, PostgresCourtLockStrategy>();
        services.AddScoped<IReservationValidator, ReservationValidator>();
        services.AddScoped<IReservationHandler, ReservationHandler>();
        services.AddScoped<IMatchHandler, MatchHandler>();

        services.AddScoped<IShuffleHandler, ShuffleHandler>();

        services.AddScoped<SeederHandler>();

        return services;
    }
}
