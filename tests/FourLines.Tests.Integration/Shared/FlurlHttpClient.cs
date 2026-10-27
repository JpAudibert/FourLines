using Flurl.Http;

namespace FourLines.Tests.Integration.Shared;

public sealed class TestHttpClient(string baseUrl) : IDisposable
{
    private readonly FlurlClient _client = new(baseUrl);

    public async Task<IFlurlResponse> SendAsync(
        string url,
        HttpMethod method,
        HttpContent? content = null,
        string? token = null,
        CancellationToken cancellationToken = default
    )
    {
        var request = _client.Request(url).AllowAnyHttpStatus();

        if (token is not null)
        {
            request = request.WithOAuthBearerToken(token);
        }

        return await request.SendAsync(method, content, cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
