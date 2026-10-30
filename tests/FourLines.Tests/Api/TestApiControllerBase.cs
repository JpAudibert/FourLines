using FourLines.Api.Controllers;
using FourLines.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FourLines.Tests.Api;

public class TestApiControllerBase
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void TryGetCurrentUser_WhenUserExists_ReturnsUserAndAdminStatus(bool isAdmin)
    {
        // Arrange
        Guid expectedUserId = Guid.NewGuid();
        Mock<ICurrentUserProvider> currentUserProvider = new();
        currentUserProvider.Setup(provider => provider.UserId).Returns(expectedUserId);
        currentUserProvider.Setup(provider => provider.IsAdmin).Returns(isAdmin);
        ApiControllerBase controller = new(NullLogger.Instance, currentUserProvider.Object);

        // Act
        bool result = controller.TryGetCurrentUser(out Guid userId, out bool actualIsAdmin);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedUserId, userId);
        Assert.Equal(isAdmin, actualIsAdmin);
    }

    [Fact]
    public void TryGetCurrentUser_WhenUserDoesNotExist_ReturnsDefaultValues()
    {
        // Arrange
        Mock<ICurrentUserProvider> currentUserProvider = new();
        currentUserProvider.Setup(provider => provider.UserId).Returns((Guid?)null);
        currentUserProvider.Setup(provider => provider.IsAdmin).Returns(true);
        ApiControllerBase controller = new(NullLogger.Instance, currentUserProvider.Object);

        // Act
        bool result = controller.TryGetCurrentUser(out Guid userId, out bool isAdmin);

        // Assert
        Assert.False(result);
        Assert.Equal(default, userId);
        Assert.False(isAdmin);
    }
}
