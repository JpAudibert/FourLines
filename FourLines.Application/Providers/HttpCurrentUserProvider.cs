using Microsoft.AspNetCore.Http;

namespace FourLines.Application.Providers;

public class HttpCurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserProvider
{
    public Guid? UserId
    {
        get
        {
            string? claim = httpContextAccessor.HttpContext?.User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            return Guid.TryParse(claim, out Guid userId) ? userId : null;
        }
    }

    public bool IsAdmin
    {
        get
        {
            string? claim = httpContextAccessor.HttpContext?.User.FindFirstValue(
                JwtTokenProvider.IsAdminRoleClaimType
            );

            return bool.TryParse(claim, out bool isAdmin) && isAdmin;
        }
    }
}
