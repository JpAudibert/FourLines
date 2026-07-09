namespace FourLines.Application.Handlers;

public class FacilityScheduleHandler(FourLinesContext context)
{
    private readonly FourLinesContext _context = context;

    public async Task<Result<FacilitySchedule>> Create(CreateFacilityScheduleDTO newSchedule)
    {
        Facility? facility = await _context.Facilities
            .FirstOrDefaultAsync(f => f.Id == newSchedule.FacilityId && f.OwnerId == newSchedule.OwnerId);
        if (facility is null)
            return Result<FacilitySchedule>.Failure(FacilitySchedulesErrorResults.CreateFacilitySchedules);

        FacilitySchedule schedule = new()
        {
            FacilityId = newSchedule.FacilityId,
            OpensAt = newSchedule.OpensAt,
            ClosesAt = newSchedule.ClosesAt,
            DayOfWeek = newSchedule.DayOfWeek,
            Facility = facility,
        };

        await _context.FacilitySchedules.AddAsync(schedule);
        await _context.SaveChangesAsync();

        return Result<FacilitySchedule>.Success(schedule);
    }

    public async Task<Result<FacilitySchedule>> Update(UpdateFacilityScheduleDTO schedule)
    {
        Facility? facility = await _context.Facilities
            .FirstOrDefaultAsync(f => f.Id == schedule.FacilityId && f.OwnerId == schedule.OwnerId);
        if (facility is null)
            return Result<FacilitySchedule>.Failure(FacilitySchedulesErrorResults.UpdateFacilitySchedules);

        int affectedRows = await _context.FacilitySchedules
            .Where(fs => fs.Id == schedule.Id && fs.FacilityId == schedule.FacilityId)
            .ExecuteUpdateAsync(fs => fs
                .SetProperty(fs => fs.OpensAt, schedule.OpensAt)
                .SetProperty(fs => fs.ClosesAt, schedule.ClosesAt)
                .SetProperty(fs => fs.DayOfWeek, schedule.DayOfWeek)
                .SetProperty(fs => fs.UpdatedAt, DateTime.UtcNow));

        if (affectedRows <= 0)
            return Result<FacilitySchedule>.Failure(FacilitySchedulesErrorResults.UpdateUnknownSchedules);

        await _context.SaveChangesAsync();

        FacilitySchedule? updatedSchedule = await _context.FacilitySchedules
            .AsNoTracking()
            .FirstOrDefaultAsync(fs => fs.Id == schedule.Id);

        return Result<FacilitySchedule>.Success(updatedSchedule!);
    }

    public async Task<Result<bool>> Delete(Guid ownerId, Guid facilityId, Guid scheduleId)
    {
        bool deleted = false;

        int affectedRows = await _context.FacilitySchedules
            .Where(fs => fs.Id == scheduleId && fs.FacilityId == facilityId && fs.Facility.OwnerId == ownerId)
            .ExecuteDeleteAsync();

        if (affectedRows <= 0)
            return Result<bool>.Failure(FacilitySchedulesErrorResults.DeleteUnknownSchedules);

        await _context.SaveChangesAsync();
        deleted = true;

        return Result<bool>.Success(deleted);
    }

    public async Task<Result<IEnumerable<FacilitySchedule>>> GetSchedules(Guid ownerId, Guid facilityId)
    {
        IEnumerable<FacilitySchedule>? schedules = await _context.FacilitySchedules
            .Where(fs => fs.FacilityId == facilityId && fs.Facility.OwnerId == ownerId)
            .AsNoTracking()
            .Select(fs => new FacilitySchedule()
            {
                Id = fs.Id,
                FacilityId = facilityId,
                OpensAt = fs.OpensAt,
                ClosesAt = fs.ClosesAt,
                DayOfWeek = fs.DayOfWeek,
                CreatedAt = fs.CreatedAt,
                UpdatedAt = fs.UpdatedAt,
            })
            .ToListAsync();

        if (!schedules.Any() || schedules is null)
            return Result<IEnumerable<FacilitySchedule>>.Failure(FacilitySchedulesErrorResults.RetrieveFacilitySchedules);

        return Result<IEnumerable<FacilitySchedule>>.Success(schedules);
    }
}
