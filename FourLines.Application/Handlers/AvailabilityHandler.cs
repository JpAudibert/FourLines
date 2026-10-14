using System.Runtime.CompilerServices;
using Dapper;
using FourLines.Infrastructure.Providers;

namespace FourLines.Application.Handlers;

public class AvailabilityHandler(ConnectionProvider connectionProvider) : IAvailabilityHandler
{
    public async IAsyncEnumerable<TimeSlot> GetTimeSlots(
        Guid courtId,
        DayOfWeek selectedDayOfTheWeek,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        const string sql =
            @"SELECT c.id AS ""CourtId"",
                     c.maintenance_period_in_minutes AS ""MaintenancePeriodInMinutes"",
                     c.renting_period_in_minutes AS ""RentingPeriodInMinutes"",
                     c.amount AS ""PriceAmount"",
                     c.currency AS ""PriceCurrency"",
                     fs.day_of_week AS ""DayOfWeek"",
                     fs.opens_at AS ""OpensAt"",
                     fs.closes_at AS ""ClosesAt""
                FROM courts c 
               INNER JOIN facilities f ON c.facility_id = f.id 
               INNER JOIN facility_schedules fs ON fs.facility_id = f.id
               WHERE c.id = @courtId
                 AND fs.day_of_week = @selectedDayOfTheWeek";

        CommandDefinition command = new(
            sql,
            parameters: new { courtId, selectedDayOfTheWeek },
            cancellationToken: cancellationToken
        );

        using var connection = connectionProvider.Provide();

        FacilityAvailability? facilityAvailability =
            await connection.QueryFirstOrDefaultAsync<FacilityAvailability>(command);

        if (facilityAvailability is null)
            yield break;

        TimeOnly current = facilityAvailability.OpensAt;
        int reservationSlot =
            facilityAvailability.MaintenancePeriodInMinutes
            + facilityAvailability.RentingPeriodInMinutes;

        while (current.AddMinutes(reservationSlot) <= facilityAvailability.ClosesAt)
        {
            TimeOnly slotEnd = current.AddMinutes(reservationSlot);

            if (slotEnd > facilityAvailability.ClosesAt)
                yield break;

            yield return new TimeSlot()
            {
                Start = current,
                End = slotEnd,
                Price = new Money(
                    facilityAvailability.PriceAmount,
                    facilityAvailability.PriceCurrency
                ),
                Available = true,
            };

            current = slotEnd;
        }
    }
}
