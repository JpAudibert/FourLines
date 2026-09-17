using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared.Seed;

public static class UserSeed
{
    public static readonly User Owner = new()
    {
        RoleId = RoleSeed.Owner.Id,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    public static readonly User Player = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Jane Smith",
        Email = "jane.smith@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.211-89",
    };

    public static readonly User Player2 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Jane Smith",
        Email = "jane.smith2@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.212-89",
    };
    public static readonly User Player3 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Jane Smith",
        Email = "jane.smith3@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.213-89",
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Users.AddRangeAsync(Owner, Player, Player2, Player3);

        await context.SaveChangesAsync();
    }
}
