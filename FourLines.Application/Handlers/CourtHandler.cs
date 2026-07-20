namespace FourLines.Application.Handlers;

public class CourtHandler(FourLinesContext context)
{
    private readonly FourLinesContext _context = context;

    public async Task<Result<Court>> Create(CreateCourtDTO newCourt)
    {
        Facility? facility = await _context.Facilities.FirstOrDefaultAsync(f =>
            f.Id == newCourt.FacilityId && f.OwnerId == newCourt.OwnerId
        );
        if (facility is null)
            return Result<Court>.Failure(CourtsErrorResults.CreateUnknownFacility);

        Sport? sport = await _context.Sports.FirstOrDefaultAsync(s => s.Id == newCourt.SportId);
        if (sport is null)
            return Result<Court>.Failure(CourtsErrorResults.CreateUnknownSport);

        Court court = new()
        {
            FacilityId = newCourt.FacilityId,
            SportId = newCourt.SportId,
            Name = newCourt.Name,
            IsActive = newCourt.IsActive,
            Facility = facility,
            Sport = sport,
        };

        await _context.Courts.AddAsync(court);
        await _context.SaveChangesAsync();

        return Result<Court>.Success(court);
    }

    public async Task<Result<Court>> Update(UpdateCourtDTO court)
    {
        Facility? facility = await _context.Facilities.FirstOrDefaultAsync(f =>
            f.Id == court.FacilityId
        );
        if (facility is null)
            return Result<Court>.Failure(CourtsErrorResults.UpdateUnknownFacility);

        int affectedRows = await _context
            .Courts.Where(c =>
                c.Id == court.Id
                && c.Facility.Id == court.FacilityId
                && c.Facility.OwnerId == court.OwnerId
            )
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(c => c.Name, court.Name)
                    .SetProperty(c => c.IsActive, court.IsActive)
            );

        if (affectedRows <= 0)
            return Result<Court>.Failure(CourtsErrorResults.UpdateCourtDoesNotExist);

        await _context.SaveChangesAsync();

        Court? updatedCourt = await _context.Courts.FindAsync(court.Id);

        return Result<Court>.Success(updatedCourt!);
    }

    public async Task<Result<bool>> Delete(Guid ownerId, Guid facilityId, Guid courtId)
    {
        bool deleted = false;
        int affectedRows = await _context
            .Courts.Where(c =>
                c.Id == courtId && c.Facility.Id == facilityId && c.Facility.OwnerId == ownerId
            )
            .ExecuteDeleteAsync();

        if (affectedRows <= 0)
            return Result<bool>.Failure(CourtsErrorResults.DeleteCourtDoesNotExist);

        await _context.SaveChangesAsync();
        deleted = true;

        return Result<bool>.Success(deleted);
    }

    public async Task<Result<Court>> GetFacility(Guid ownerId, Guid facilityId, Guid courtId)
    {
        Court? court = await _context.Courts.FirstOrDefaultAsync(c =>
            c.Id == courtId && c.Facility.Id == facilityId && c.Facility.OwnerId == ownerId
        );

        if (court is null)
            return Result<Court>.Failure(CourtsErrorResults.RetrieveGetCourtDoesNotExist);

        return Result<Court>.Success(court);
    }

    public async Task<Result<IEnumerable<Court>>> GetAllCourtsFromFacility(
        Guid ownerId,
        Guid facilityId
    )
    {
        IEnumerable<Court?> courts = await _context
            .Courts.Where(c => c.Facility.Id == facilityId && c.Facility.OwnerId == ownerId)
            .ToListAsync();

        if (courts is null || !courts.Any())
            return Result<IEnumerable<Court>>.Failure(
                CourtsErrorResults.RetrieveGetCourtDoesNotExist
            );

        return Result<IEnumerable<Court>>.Success(courts!);
    }
}
