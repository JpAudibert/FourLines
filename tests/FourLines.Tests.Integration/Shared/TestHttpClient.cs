using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.Users;

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

    public async Task<string> LoginAsync(LoginRequest body, int version = 1)
    {
        string url = $"/api/v{version}/auth";
        var request = _client.Request(url);

        string result = await request.PostAsync(JsonContent.Create(body)).Result.GetStringAsync();

        return result;
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
