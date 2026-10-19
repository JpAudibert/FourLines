# Reservation Process

## Description
This document is meant to describe the reservation process and its business rules

## Routes

### Get All Reservations From User
```
GET - /api/v{version}/Reservations/users/{userId}
```

### Get All Reservations From Court
```
GET - /api/v{version}/Reservations/courts/{courtId}
```

### Get One Reservations From User
```
GET - /api/v{version}/Reservations/{reservationId}/users/{userId}
```

### Get Available Times To Play In Court
```
GET - /api/v{version}/Reservations/courts/{courtId}/selected-date/{selectedDate}
```
The request expects:
* Route params:
```json
    {
        "courtId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
        "selectedDate": "2026-10-19"
    }
```
* The handler starts creating the slots for availability for that day of week, from the begining to the end of the day. It connects:
    * Courts
    * Facilities
    * Facility Schedules
* It creates the `time slots, all available`
* `Now` is defined
    * It takes UTC time and passes it to facility time (GMT-3 -> for Brazil/GMT-5 -> for Bogotá)
* Queries Reservation table to find something for the date and for the court that is not cancelled:
```csharp
Reservations.Where(r =>
    r.CourtId == courtId
    && DateOnly.FromDateTime(r.Period.Start.DateTime) == selectedDate
    && r.Status != ReservationStatus.Cancelled
)
.Select(r => new Reservation { Period = r.Period })
```
* Iterates through the time slot's list and flag these as unavailable
    * Slots in the past
    * Slots that has a reservation overlapping
* Returns a `Result.Success` with the court availability


### Create A Reservation For User
```
POST - /api/v{version}/Reservations/users/{userId}
```
The request expects:
* Route params: `userId`
* Body
```json
    {
        "courtId": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
        "period": {
            "start": "2026-10-20T14:00:00",
            "end": "2026-10-20T15:00:00",
            "duration": "01:00:00"
        },
        "status": 1
    }
```
* The handler validates the following conditions:
    * Are dates valid?
    * Are Start and End dates in the past?
    * Is the duration different than what is configured in the DB?
    * Is the status valid?

With **all succeeded** a ``transation`` is started

* It starts locking and getting the court to be reserved
    * Strategy used is a `SELECT FOR UPDATE` - pure SQL used, is PostgreSQL attached
    * If the query returns nothing, `Result.Failure` is exposed
* Gets the user
    * If the query returns nothing, `Result.Failure` is exposed
* Gets the facility schedule with its facility
    * If the query returns nothing, `Result.Failure` is exposed
    * Validates if facility ``TimeZoneId`` is valid - uses IANA pattern
        * TZConvert.GetTimeZoneInfo(timeZoneId);
        * If the validation fails, it throws `TimeZoneNotFoundException` and `InvalidTimeZoneException`
* Transforms the `Period` received from the request `to UTC`
    * **Reservations are saved always in UTC, it's a point in time**
* Creates the `Reservation in memory`
* Verify if there is an overlapping reservation
    * If there is some, `Result.Failure` is exposed

```csharp
Reservations.Where(r =>
    r.CourtId == newReservation.CourtId
    && r.Period.Start < utcReservationPeriod.End
    && r.Period.End > utcReservationPeriod.Start
    && r.Status != ReservationStatus.Cancelled
)
```
* Creates a `Match in memory`
* Adds `Reservation` and `Memory` to the context
* Save changes to the DB
* Commit the transaction
* Exposes a Result.Success


### Update Status From Reservation
```
PATCH - /api/v{version}/Reservations/{reservationId}/users/{userId}
```

### Delete  A Reservation From User
```
DELETE - /api/v{version}/Reservations/{reservationId}/users/{userId}
```
