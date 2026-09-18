using FourLines.Application.DTOs.Matches;
using FourLines.Application.DTOs.Matches.Interfaces;

namespace FourLines.Application.Handlers;

public class MatchHandler(FourLinesContext context) : IMatchHandler
{
    public async Task<Result<Match?>> GetMatch(
        Guid matchId,
        CancellationToken cancellationToken = default
    )
    {
        Match? match = await context
            .Matches.Include(r => r.Reservation)
            .Include(s => s.Sport)
            .FirstOrDefaultAsync(m => m.Id == matchId, cancellationToken: cancellationToken);

        return Result<Match?>.Success(match);
    }

    public async Task<Result<Match>> UpdateMatchName(
        UpdateMatchNameDTO updateMatchName,
        CancellationToken cancellationToken = default
    )
    {
        int affectedRows = await context
            .Matches.Where(m => m.Id == updateMatchName.MatchId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(m => m.Name, updateMatchName.NewName),
                cancellationToken: cancellationToken
            );

        if (affectedRows <= 0)
            return Result<Match>.Failure(MatchesErrorResults.MatchNotFound);

        Match updatedMatch = await context
            .Matches.Include(r => r.Reservation)
            .Include(s => s.Sport)
            .FirstAsync(m => m.Id == updateMatchName.MatchId, cancellationToken: cancellationToken);

        updatedMatch.Name = updateMatchName.NewName;

        return Result<Match>.Success(updatedMatch);
    }

    public async Task<Result<MatchesUsers>> Ingress(
        ICreateIngressDTO ingress,
        CancellationToken cancellationToken = default
    )
    {
        Match? match = await context.Matches.FirstOrDefaultAsync(
            m => m.Id == ingress.MatchId && m.Code == ingress.Code,
            cancellationToken: cancellationToken
        );
        if (match is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressMatchNotFound);

        User? user = await context.Users.FirstOrDefaultAsync(
            u => u.Id == ingress.UserId,
            cancellationToken: cancellationToken
        );
        if (user is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressUserNotFound);

        MatchesUsers matchesUsers = new()
        {
            MatchId = ingress.MatchId,
            UserId = ingress.UserId,
            IsFixedPosition = false,
            Match = match,
            User = user,
        };

        await context.MatchesUsers.AddAsync(matchesUsers, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<MatchesUsers>.Success(matchesUsers);
    }

    public async Task<Result<MatchesUsers>> IngressAsFixedPosition(
        ICreateIngressDTO ingress,
        CancellationToken cancellationToken = default
    )
    {
        Match? match = await context
            .Matches.Include(s => s.Sport)
            .FirstOrDefaultAsync(
                m => m.Id == ingress.MatchId && m.Code == ingress.Code,
                cancellationToken: cancellationToken
            );
        if (match is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressMatchNotFound);

        if (!match.Sport.HasFixedPosition)
            return Result<MatchesUsers>.Failure(
                MatchesErrorResults.IngressSportDoesNotHaveFixedGoalKeeper
            );

        User? user = await context.Users.FirstOrDefaultAsync(
            u => u.Id == ingress.UserId,
            cancellationToken: cancellationToken
        );
        if (user is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressUserNotFound);

        MatchesUsers matchesUsers = new()
        {
            MatchId = ingress.MatchId,
            UserId = ingress.UserId,
            IsFixedPosition = true,
            Match = match,
            User = user,
        };

        await context.MatchesUsers.AddAsync(matchesUsers, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<MatchesUsers>.Success(matchesUsers);
    }

    public async Task<Result<bool>> LeaveMatch(
        LeaveMatchDTO leaveMatch,
        CancellationToken cancellationToken = default
    )
    {
        MatchesUsers? userInTheMatch = await context.MatchesUsers.FirstOrDefaultAsync(
            mu => mu.MatchId == leaveMatch.MatchId && mu.UserId == leaveMatch.UserId,
            cancellationToken: cancellationToken
        );

        if (userInTheMatch is null)
            return Result<bool>.Failure(MatchesErrorResults.LeaveMatchNotFound);

        context.MatchesUsers.Remove(userInTheMatch);
        await context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
