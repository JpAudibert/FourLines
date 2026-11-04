using System.Text.Json;
using FourLines.Tests.Integration.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Tests.Integration.ExceptionHandlers;

[Collection(FourLinesCollection.Name)]
public class GlobalExceptionHandlerTests(FourLinesFixture fixture)
{
    [Theory]
    [InlineData("validation", 400, "ValidationException", "Validation failed.")]
    [InlineData("unauthorized", 401, "UnauthorizedAccessException", "Access denied.")]
    [InlineData("unexpected", 500, "InvalidOperationException", "Unexpected failure.")]
    public async Task RequestExceptions_AreHandledAsProblemDetails(
        string exceptionKind,
        int expectedStatusCode,
        string expectedType,
        string expectedDetail
    )
    {
        var response = await fixture.HttpClient.SendAsync(
            url: $"/_test/exception/{exceptionKind}",
            method: HttpMethod.Get
        );
        string responseBody = await response.GetStringAsync();
        ProblemDetails? problemDetails = JsonSerializer.Deserialize<ProblemDetails>(
            responseBody,
            new JsonSerializerOptions(JsonSerializerDefaults.Web)
        );

        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal(expectedStatusCode, problemDetails.Status);
        Assert.Equal("An unexpected error occurred.", problemDetails.Title);
        Assert.Equal(expectedType, problemDetails.Type);
        Assert.Equal(expectedDetail, problemDetails.Detail);
        Assert.Equal("GET /_test/exception/" + exceptionKind, problemDetails.Instance);
        Assert.Contains("requestId", problemDetails.Extensions.Keys);
        Assert.Contains("traceId", problemDetails.Extensions.Keys);
    }
}
