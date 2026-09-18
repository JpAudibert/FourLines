namespace FourLines.Application.Interfaces;

public interface IAuthenticationHandler
{
    Task<Result<string>> Authenticate(
        AuthenticationDTO request,
        CancellationToken cancellationToken = default
    );
}
