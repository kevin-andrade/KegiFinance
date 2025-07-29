using KegiFin.Api.Data;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Helpers.Testing;

public static class HandlerTestHelper<THandler> where THandler : class
{
    public static Mock<IAppDbContext> CreateMockContext() => new();
    
    public static THandler CreateHandler(
        Mock<IAppDbContext> mockContext,
        Mock<ILogger<THandler>> mockLogger,
        Func<IAppDbContext, ILogger<THandler>, THandler> factory)
            => factory(mockContext.Object, mockLogger.Object);
    
    public static void VerifyLog(Mock<ILogger<THandler>> loggerMock, LogLevel level, string expectedMessage)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}