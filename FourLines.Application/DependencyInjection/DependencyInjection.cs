using FourLines.Application.Providers;
using FourLines.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITokenProvider, JwtTokenProvider>();

        services.AddSingleton<PasswordHasher<User>>();

        services.AddScoped<IPasswordHashProvider, PasswordHashProvider>();
        services.AddScoped<UserService>();

        return services;
    }
}
