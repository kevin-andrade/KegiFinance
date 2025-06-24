using KegiFin.Api.Data;
using KegiFin.Api.Handlers;
using KegiFin.Core.Requests.Categories;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;

public static class CategoryHandlerTestHelper
{
    public static Mock<IAppDbContext> CreateMockContext() => new();
    public static CategoryHandler CreateHandler(Mock<IAppDbContext> mockContext,
        Mock<ILogger<CategoryHandler>> mockLogger) =>
        new(mockContext.Object, mockLogger.Object);

    #region Requests

    public static CreateCategoryRequest GetValidCreateRequest()
        => new()
        {
            Name = "Category Mock",
            Description = "Test Mock"
        };
    
    public static UpdateCategoryRequest GetValidUpdateRequest(int id = 1, string userId = "123")
        => new()
        {
            Id = id,
            UserId = userId,
            Name = $"Category Mock Update {id}",
            Description = $"Description for category {id}"
        };
    
    public static DeleteCategoryRequest GetValidDeleteRequest(int id = 1, string userId = "123")
        => new()
        {
            Id = id,
            UserId = userId,
        };
    
    public static GetCategoryByIdRequest GetValidCategoryByIdRequest(int id = 1, string userId = "123")
        => new()
        {
            Id = id,
            UserId = userId,
        };
    
    public static GetAllCategoriesRequest GetValidAllCategoriesRequest(string userId = "123", int pageNumber = 1, int pageSize = 10)
        => new()
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            
        };

    #endregion
    
    public static void VerifyLog(Mock<ILogger<CategoryHandler>> loggerMock, LogLevel level, string expectedMessage)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}