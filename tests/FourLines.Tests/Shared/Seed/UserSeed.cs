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

    public static readonly User PlayerToShuffle1 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Alice Johnson",
        Email = "alice.johnson5@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1992, 8, 20),
        Phone = "55 54 9 7777-7777",
        RegistrationNumber = "741.263.958-43",
    };

    public static readonly User PlayerToShuffle2 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Michael Brown",
        Email = "michael.brown6@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1978, 11, 10),
        Phone = "55 54 9 6666-6666",
        RegistrationNumber = "162.537.849-65",
    };

    public static readonly User PlayerToShuffle3 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Robert Wilson",
        Email = "robert.wilson7@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1988, 3, 25),
        Phone = "55 54 9 5555-5555",
        RegistrationNumber = "274.681.935-17",
    };

    public static readonly User PlayerToShuffle4 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Emily Davis",
        Email = "emily.davis8@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1995, 7, 12),
        Phone = "55 54 9 4444-4444",
        RegistrationNumber = "618.425.793-31",
    };

    public static readonly User PlayerToShuffle5 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Daniel Taylor",
        Email = "daniel.taylor9@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1987, 4, 18),
        Phone = "55 54 9 3333-3333",
        RegistrationNumber = "415.728.963-52",
    };

    public static readonly User PlayerToShuffle6 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Sophia Anderson",
        Email = "sophia.anderson10@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1993, 9, 7),
        Phone = "55 54 9 2222-2222",
        RegistrationNumber = "837.514.629-73",
    };

    public static readonly User PlayerToShuffle7 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Matthew Thomas",
        Email = "matthew.thomas11@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1982, 12, 3),
        Phone = "55 54 9 1111-1111",
        RegistrationNumber = "296.843.715-94",
    };

    public static readonly User PlayerToShuffle8 = new()
    {
        RoleId = RoleSeed.Player.Id,
        Name = "Olivia Martinez",
        Email = "olivia.martinez12@example.com",
        PasswordHash =
            "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1991, 6, 22),
        Phone = "55 54 9 0000-0000",
        RegistrationNumber = "563.927.481-05",
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Users.AddRangeAsync(
            Owner,
            Player,
            Player2,
            Player3,
            PlayerToShuffle1,
            PlayerToShuffle2,
            PlayerToShuffle3,
            PlayerToShuffle4,
            PlayerToShuffle5,
            PlayerToShuffle6,
            PlayerToShuffle7,
            PlayerToShuffle8
        );

        await context.SaveChangesAsync();
    }
}
