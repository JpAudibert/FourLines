using FourLines.Application.DTOs.Courts.Interfaces;

namespace FourLines.Application.Handlers;

public class CourtHandler(FourLinesContext context) : ICourtHandler
{
    public async Task<Result<Court>> Create(
        ICreateCourtDTO newCourt,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f => f.Id == newCourt.FacilityId && f.OwnerId == newCourt.OwnerId,
            cancellationToken: cancellationToken
        );
        if (facility is null)
            return Result<Court>.Failure(CourtsErrorResults.CreateUnknownFacility);

        Sport? sport = await context.Sports.FirstOrDefaultAsync(
            s => s.Id == newCourt.SportId,
            cancellationToken: cancellationToken
        );
        if (sport is null)
            return Result<Court>.Failure(CourtsErrorResults.CreateUnknownSport);

        Court court = new()
        {
            FacilityId = newCourt.FacilityId,
            SportId = newCourt.SportId,
            Name = newCourt.Name,
            IsActive = newCourt.IsActive,
            DefaultPrice = newCourt.DefaultPrice,
            RentingPeriodInMinutes = newCourt.RentingPeriodInMinutes,
            MaintenancePeriodInMinutes = newCourt.MaintenancePeriodInMinutes,
            Facility = facility,
            Sport = sport,
        };

        await context.Courts.AddAsync(court, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<Court>.Success(court);
    }

    public async Task<Result<Court>> Update(
        IUpdateCourtDTO court,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f => f.Id == court.FacilityId,
            cancellationToken: cancellationToken
        );
        if (facility is null)
            return Result<Court>.Failure(CourtsErrorResults.UpdateUnknownFacility);

        int affectedRows = await context
            .Courts.Where(c => c.Id == court.Id && c.Facility.Id == court.FacilityId)
            .ExecuteUpdateAsync(
                setters =>
                    setters
                        .SetProperty(c => c.Name, court.Name)
                        .SetProperty(c => c.IsActive, court.IsActive)
                        .SetProperty(c => c.SportId, court.SportId)
                        .SetProperty(c => c.DefaultPrice, court.DefaultPrice)
                        .SetProperty(c => c.RentingPeriodInMinutes, court.RentingPeriodInMinutes)
                        .SetProperty(
                            c => c.MaintenancePeriodInMinutes,
                            court.MaintenancePeriodInMinutes
                        ),
                cancellationToken: cancellationToken
            );

        if (affectedRows <= 0)
            return Result<Court>.Failure(CourtsErrorResults.UpdateCourtDoesNotExist);

        await context.SaveChangesAsync(cancellationToken);

        Court? updatedCourt = await context.Courts.FindAsync(
            [court.Id],
            cancellationToken: cancellationToken
        );

        return Result<Court>.Success(updatedCourt!);
    }

    public async Task<Result<bool>> Delete(
        IDeleteCourtDTO deleteDto,
        CancellationToken cancellationToken = default
    )
    {
        bool deleted = false;
        int affectedRows = await context
            .Courts.Where(c => c.Id == deleteDto.CourtId && c.Facility.Id == deleteDto.FacilityId)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        if (affectedRows <= 0)
            return Result<bool>.Failure(CourtsErrorResults.DeleteCourtDoesNotExist);

        await context.SaveChangesAsync(cancellationToken);
        deleted = true;

        return Result<bool>.Success(deleted);
    }

    public async Task<Result<Court>> GetCourtFromFacility(
        Guid facilityId,
        Guid courtId,
        CancellationToken cancellationToken = default
    )
    {
        Court? court = await context.Courts.FirstOrDefaultAsync(
            c => c.Id == courtId && c.Facility.Id == facilityId,
            cancellationToken: cancellationToken
        );

        if (court is null)
            return Result<Court>.Failure(CourtsErrorResults.RetrieveGetCourtDoesNotExist);

        return Result<Court>.Success(court);
    }

    public async Task<Result<IEnumerable<Court>>> GetAllCourtsFromFacility(
        Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<Court?> courts = await context
            .Courts.Where(c => c.Facility.Id == facilityId)
            .ToListAsync(cancellationToken: cancellationToken);

        if (courts is null || !courts.Any())
            return Result<IEnumerable<Court>>.Failure(
                CourtsErrorResults.RetrieveGetCourtDoesNotExist
            );

        return Result<IEnumerable<Court>>.Success(courts!);
    }
}
