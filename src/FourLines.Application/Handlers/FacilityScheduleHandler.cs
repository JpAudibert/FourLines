using FourLines.Application.DTOs.FacilitySchedules;

namespace FourLines.Application.Handlers;

public class FacilityScheduleHandler(FourLinesContext context) : IFacilityScheduleHandler
{
    public async Task<Result<FacilitySchedule>> Create(
        CreateFacilityScheduleDTO newSchedule,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f =>
                f.Id == newSchedule.FacilityId
                && (newSchedule.IsAdmin || f.OwnerId == newSchedule.OwnerId),
            cancellationToken: cancellationToken
        );
        if (facility is null)
            return Result<FacilitySchedule>.Failure(
                FacilitySchedulesErrorResults.CreateFacilitySchedules
            );

        FacilitySchedule schedule = new()
        {
            FacilityId = newSchedule.FacilityId,
            OpensAt = newSchedule.OpensAt,
            ClosesAt = newSchedule.ClosesAt,
            DayOfWeek = newSchedule.DayOfWeek,
            Facility = facility,
        };

        await context.FacilitySchedules.AddAsync(schedule, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<FacilitySchedule>.Success(schedule);
    }

    public async Task<Result<IEnumerable<FacilitySchedule>>> CreateMultiple(
        List<CreateFacilityScheduleDTO> newSchedules,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f =>
                f.Id == newSchedules[0].FacilityId
                && (newSchedules[0].IsAdmin || f.OwnerId == newSchedules[0].OwnerId),
            cancellationToken: cancellationToken
        );
        if (
            facility is null
            || newSchedules.Any(schedule =>
                schedule.FacilityId != facility.Id
                || schedule.OwnerId != newSchedules[0].OwnerId
                || schedule.IsAdmin != newSchedules[0].IsAdmin
            )
        )
            return Result<IEnumerable<FacilitySchedule>>.Failure(
                FacilitySchedulesErrorResults.CreateFacilitySchedules
            );

        List<FacilitySchedule> schedules = [];
        foreach (var schedule in newSchedules)
        {
            schedules.Add(
                new()
                {
                    FacilityId = schedule.FacilityId,
                    DayOfWeek = schedule.DayOfWeek,
                    OpensAt = schedule.OpensAt,
                    ClosesAt = schedule.ClosesAt,
                    Facility = facility,
                }
            );
        }

        await context.FacilitySchedules.AddRangeAsync(schedules, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<IEnumerable<FacilitySchedule>>.Success(schedules);
    }

    public async Task<Result<FacilitySchedule>> Update(
        UpdateFacilityScheduleDTO schedule,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f =>
                f.Id == schedule.FacilityId
                && (schedule.IsAdmin || f.OwnerId == schedule.OwnerId),
            cancellationToken: cancellationToken
        );
        if (facility is null)
            return Result<FacilitySchedule>.Failure(
                FacilitySchedulesErrorResults.UpdateUnknownFacility
            );

        int affectedRows = await context
            .FacilitySchedules.Where(fs =>
                fs.Id == schedule.Id
                && fs.FacilityId == schedule.FacilityId
                && (schedule.IsAdmin || fs.Facility.OwnerId == schedule.OwnerId)
            )
            .ExecuteUpdateAsync(
                fs =>
                    fs.SetProperty(fs => fs.OpensAt, schedule.OpensAt)
                        .SetProperty(fs => fs.ClosesAt, schedule.ClosesAt)
                        .SetProperty(fs => fs.DayOfWeek, schedule.DayOfWeek)
                        .SetProperty(fs => fs.UpdatedAt, DateTime.UtcNow),
                cancellationToken: cancellationToken
            );

        if (affectedRows <= 0)
            return Result<FacilitySchedule>.Failure(
                FacilitySchedulesErrorResults.UpdateUnknownSchedules
            );

        await context.SaveChangesAsync(cancellationToken);

        FacilitySchedule? updatedSchedule = await context
            .FacilitySchedules.AsNoTracking()
            .FirstOrDefaultAsync(fs => fs.Id == schedule.Id, cancellationToken: cancellationToken);

        return Result<FacilitySchedule>.Success(updatedSchedule!);
    }

    public async Task<Result<bool>> Delete(
        DeleteFacilityScheduleDTO deleteDto,
        CancellationToken cancellationToken = default
    )
    {
        bool deleted = false;

        int affectedRows = await context
            .FacilitySchedules.Where(fs =>
                fs.Id == deleteDto.ScheduleId
                && fs.FacilityId == deleteDto.FacilityId
                && (deleteDto.IsAdmin || fs.Facility.OwnerId == deleteDto.OwnerId)
            )
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        if (affectedRows <= 0)
            return Result<bool>.Failure(FacilitySchedulesErrorResults.DeleteUnknownSchedules);

        await context.SaveChangesAsync(cancellationToken);
        deleted = true;

        return Result<bool>.Success(deleted);
    }

    public async Task<Result<IEnumerable<FacilitySchedule>>> GetSchedules(
        Guid ownerId,
        bool isAdmin,
        Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<FacilitySchedule>? schedules = await context
            .FacilitySchedules.Where(fs =>
                fs.FacilityId == facilityId && (isAdmin || fs.Facility.OwnerId == ownerId)
            )
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
            .ToListAsync(cancellationToken: cancellationToken);

        if (schedules is null || !schedules.Any())
            return Result<IEnumerable<FacilitySchedule>>.Failure(
                FacilitySchedulesErrorResults.RetrieveFacilitySchedules
            );

        return Result<IEnumerable<FacilitySchedule>>.Success(schedules);
    }
}
