namespace FourLines.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITokenProvider, JwtTokenProvider>();

        services.AddSingleton<PasswordHasher<User>>();

        services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();
        services.AddScoped<AuthenticationHandler>();

        services.AddScoped<UserHandler>();
        services.AddScoped<IFacilityHandler, FacilityHandler>();
        services.AddScoped<IFacilityScheduleHandler, FacilityScheduleHandler>();
        services.AddScoped<ICourtHandler, CourtHandler>();

        services.AddScoped<IReservationValidator, ReservationValidator>();
        services.AddScoped<IReservationHandler, ReservationHandler>();

        services.AddScoped<SeederHandler>();

        return services;
    }
}
