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
        services.AddScoped<FacilityHandler>();
        services.AddScoped<FacilityScheduleHandler>();
        services.AddScoped<CourtHandler>();

        services.AddScoped<SeederHandler>();

        return services;
    }
}
