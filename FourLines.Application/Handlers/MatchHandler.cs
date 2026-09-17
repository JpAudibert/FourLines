using FourLines.Application.DTOs.Matches;
using FourLines.Application.DTOs.Matches.Interfaces;

namespace FourLines.Application.Handlers;

public class MatchHandler(FourLinesContext context) : IMatchHandler
{
    public async Task<Result<Match?>> GetMatch(Guid matchId)
    {
        Match? match = await context.Matches
            .Include(r => r.Reservation)
            .Include(s => s.Sport)
            .FirstOrDefaultAsync(m => m.Id == matchId);

        return Result<Match?>.Success(match);
    }

    public async Task<Result<Match>> UpdateMatchName(UpdateMatchNameDTO updateMatchName)
    {
        int affectedRows = await context.Matches
            .Where(m => m.Id == updateMatchName.MatchId)
            .ExecuteUpdateAsync(setters => 
                setters.SetProperty(m => m.Name, updateMatchName.NewName)
            );

        if (affectedRows <= 0)
            return Result<Match>.Failure(MatchesErrorResults.MatchNotFound);

        Match updatedMatch = await context.Matches
            .Include(r => r.Reservation)
            .Include(s => s.Sport)
            .FirstAsync(m => m.Id == updateMatchName.MatchId);

        updatedMatch.Name = updateMatchName.NewName;

        return Result<Match>.Success(updatedMatch);
    }

    public async Task<Result<MatchesUsers>> Ingress(ICreateIngressDTO ingress)
    {
        Match? match = await context.Matches.FirstOrDefaultAsync(m => m.Id == ingress.MatchId && m.Code == ingress.Code);
        if (match is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressMatchNotFound);

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == ingress.UserId);
        if (user is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressUserNotFound);

        MatchesUsers matchesUsers = new()
        {
            MatchId = ingress.MatchId,
            UserId = ingress.UserId,
            IsFixedPosition = false,
            Match = match,
            User = user
        };

        await context.MatchesUsers.AddAsync(matchesUsers);
        await context.SaveChangesAsync();

        return Result<MatchesUsers>.Success(matchesUsers);
    }

    public async Task<Result<MatchesUsers>> IngressAsFixedPosition(ICreateIngressDTO ingress)
    {
        Match? match = await context.Matches
            .Include(s => s.Sport)
            .FirstOrDefaultAsync(m => m.Id == ingress.MatchId && m.Code == ingress.Code);
        if (match is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressMatchNotFound);

        if(!match.Sport.HasFixedPosition)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressSportDoesNotHaveFixedGoalKeeper);

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == ingress.UserId);
        if (user is null)
            return Result<MatchesUsers>.Failure(MatchesErrorResults.IngressUserNotFound);

        MatchesUsers matchesUsers = new()
        {
            MatchId = ingress.MatchId,
            UserId = ingress.UserId,
            IsFixedPosition = true,
            Match = match,
            User = user
        };

        await context.MatchesUsers.AddAsync(matchesUsers);
        await context.SaveChangesAsync();

        return Result<MatchesUsers>.Success(matchesUsers);
    }

    public async Task<Result<bool>> LeaveMatch(LeaveMatchDTO leaveMatch)
    {
        MatchesUsers? userInTheMatch = await context.MatchesUsers
            .FirstOrDefaultAsync(mu =>
                mu.MatchId == leaveMatch.MatchId &&
                mu.UserId == leaveMatch.UserId);

        if (userInTheMatch is null)
            return Result<bool>.Failure(MatchesErrorResults.LeaveMatchNotFound);

        context.MatchesUsers.Remove(userInTheMatch);
        await context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}
