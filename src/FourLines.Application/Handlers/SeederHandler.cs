namespace FourLines.Application.Handlers;

[ExcludeFromCodeCoverage]
public class SeederHandler(FourLinesContext context)
{
    public async Task Seed(CancellationToken cancellationToken = default)
    {
        using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
        using (context)
        {
            try
            {
                await SeedRolesAsync(cancellationToken);
                await SeedSportsAsync(cancellationToken);
                await SeedUsers(cancellationToken);
                await SeedFacilities(cancellationToken);
                await SeedCourts(cancellationToken);
                await SeedFacilitySchedules(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }

    public async Task SeedRolesAsync(CancellationToken cancellationToken = default)
    {
        if (!await ValidateIsEmpty<Role>(cancellationToken))
            return;

        var roles = new List<Role>
        {
            new() { Name = "Admin" },
            new() { Name = "Player" },
            new() { Name = "Facility Owner" },
            new() { Name = "Coach" },
            new() { Name = "Manager" },
        };

        await context.Roles.AddRangeAsync(roles, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SeedSportsAsync(CancellationToken cancellationToken = default)
    {
        if (!await ValidateIsEmpty<Sport>(cancellationToken))
            return;

        var sports = new List<Sport>
        {
            new()
            {
                Name = "Football",
                Indoor = false,
                StartingPlayersCount = 22,
                MaxPlayersCount = 26,
            },
            new()
            {
                Name = "Futsal",
                Indoor = true,
                StartingPlayersCount = 10,
                MaxPlayersCount = 14,
            },
            new()
            {
                Name = "Basketball",
                Indoor = true,
                StartingPlayersCount = 10,
                MaxPlayersCount = 14,
            },
            new()
            {
                Name = "Volleyball",
                Indoor = true,
                StartingPlayersCount = 12,
                MaxPlayersCount = 16,
            },
            new()
            {
                Name = "Tennis",
                Indoor = false,
                StartingPlayersCount = 2,
                MaxPlayersCount = 4,
            },
            new()
            {
                Name = "Padel",
                Indoor = true,
                StartingPlayersCount = 4,
                MaxPlayersCount = 6,
            },
        };

        await context.Sports.AddRangeAsync(sports, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SeedUsers(CancellationToken cancellationToken = default)
    {
        if (await ValidateIsEmpty<User>(cancellationToken) == false)
            return;

        int rolesCount = context.Roles.Count();
        List<Role> roles = [.. context.Roles];
        int maxUsersPerRole = 3;
        List<User> users = [];

        for (int i = 0; i < rolesCount; i++)
        {
            for (int j = 0; j < maxUsersPerRole; j++)
            {
                string password = "123456";
                User newUser = new()
                {
                    RoleId = roles[i].Id,
                    Name = $"User {i + j} {roles[i].Name}",
                    Email = $"user{i + j}_{roles[i].Name.ToLower()}@email.com",
                    Birthday = new DateOnly(1990 + i, 1, 1),
                    Phone = $"+55 11 99999-00{(i * rolesCount) + j + 1:D2}",
                    RegistrationNumber = $"USR{(i * rolesCount) + j + 1:D6}",
                    IsActive = true,
                };
                newUser.PasswordHash = new PasswordHasher<User>().HashPassword(newUser, password);

                users.Add(newUser);
            }
        }

        await context.Users.AddRangeAsync(users, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SeedFacilities(CancellationToken cancellationToken = default)
    {
        if (!await ValidateIsEmpty<Facility>(cancellationToken))
            return;

        int sportsCount = context.Sports.Count();
        List<User> facilityOwners = [.. context.Users.Where(u => u.Role.Name == "Facility Owner")];
        List<Facility> facilities = [];

        for (int i = 0; i < facilityOwners.Count; i++)
        {
            for (int j = 0; j < sportsCount; j++)
            {
                Facility facility = new()
                {
                    OwnerId = facilityOwners[i].Id,
                    Name = $"Facility {i + 1} - {facilityOwners[i].Name}",
                    Address = $"Address {i + 1} - {facilityOwners[i].Name}",
                    City = $"City {i + 1}",
                    State = $"State {i + 1}",
                    ZipCode = $"00000-00{(i * sportsCount) + j + 1:D2}",
                    RegistrationNumber = $"FAC{(i * sportsCount) + j + 1:D6}",
                };

                facilities.Add(facility);
            }
        }

        await context.Facilities.AddRangeAsync(facilities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SeedCourts(CancellationToken cancellationToken = default)
    {
        if (!await ValidateIsEmpty<Court>(cancellationToken))
            return;

        int sportsCount = context.Sports.Count();
        List<Facility> facilities = [.. context.Facilities];
        int maxCourtsPerSport = 3;
        List<Court> courts = [];
        for (int i = 0; i < sportsCount; i++)
        {
            for (int j = 0; j < maxCourtsPerSport; j++)
            {
                courts.Add(
                    new Court()
                    {
                        FacilityId =
                            facilities.ElementAtOrDefault(j % facilities.Count)?.Id
                            ?? Guid.NewGuid(),
                        SportId = context.Sports.ElementAtOrDefault(i)?.Id ?? Guid.NewGuid(),
                        Name = $"Court {i + 1} - {facilities[i].Name}",
                        IsActive = true,
                    }
                );
            }
        }
        await context.Courts.AddRangeAsync(courts, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SeedFacilitySchedules(CancellationToken cancellationToken = default)
    {
        if (!await ValidateIsEmpty<FacilitySchedule>(cancellationToken))
            return;

        List<Facility> facilities = [.. context.Facilities];
        List<FacilitySchedule> schedules = [];

        foreach (var facility in facilities)
        {
            for (int day = 1; day < 7; day++)
            {
                schedules.Add(
                    new FacilitySchedule()
                    {
                        FacilityId = facility.Id,
                        DayOfWeek = (DayOfWeek)day,
                        OpensAt = new TimeOnly(8, 0),
                        ClosesAt = new TimeOnly(22, 0),
                    }
                );
            }
        }

        await context.FacilitySchedules.AddRangeAsync(schedules, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ValidateIsEmpty<T>(CancellationToken cancellationToken = default)
        where T : class
    {
        return await context.Set<T>().AnyAsync(cancellationToken: cancellationToken) == false;
    }
}
