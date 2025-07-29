using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Helpers.Mocking.Logging;

public static class LoggerMockHelper
{
    public static Mock<ILogger<TEntity>> GetMockLogger<TEntity>() => new();
}