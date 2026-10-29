using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared.Seed;

public static class RoleSeed
{
    public static readonly Role Owner = new() { Name = RoleConstants.FacilityOwner };

    public static readonly Role Player = new() { Name = RoleConstants.Player };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Roles.AddRangeAsync(Owner, Player);

        await context.SaveChangesAsync();
    }
}
