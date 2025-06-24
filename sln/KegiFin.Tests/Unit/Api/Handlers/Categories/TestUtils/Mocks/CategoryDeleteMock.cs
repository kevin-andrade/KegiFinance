using KegiFin.Api.Data;
using KegiFin.Api.Handlers;
using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Helpers.Mocking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Mocks;

public static class CategoryDeleteMock
{
    private static Mock<DbSet<Category>> GetMockCategoryDbSet(List<Category> categories)
    {
        var mockSet = DbSetMockHelper.CreateMockDbSet(categories);
        
        mockSet.Setup(m => m.Remove(It.IsAny<Category>()))
            .Callback<Category>(category =>
            {
                var existCategory = categories.FirstOrDefault(
                    x => x.Id == category.Id && x.UserId == category.UserId);
                if (existCategory is null) return;
                categories.Remove(existCategory);
            });
        
        return mockSet;
    }

    public static Mock<IAppDbContext> GetSuccessMock(List<Category> categories)
    {
        var mockContext = CategoryHandlerTestHelper.CreateMockContext();
        var mockSet = GetMockCategoryDbSet(categories);
        
        mockContext.Setup(x => x.Categories).Returns(mockSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        
        return mockContext;
    }
    
    public static Mock<IAppDbContext> GetNotFoundMock(List<Category> categories)
    {
        var mockContext = CategoryHandlerTestHelper.CreateMockContext();
        var mockSet = GetMockCategoryDbSet(categories);
        
        mockContext.Setup(x => x.Categories).Returns(mockSet.Object);
        
        return mockContext;
    }

    public static Mock<IAppDbContext> GetFailMock(List<Category> categories)
    {
        var mockContext = CategoryHandlerTestHelper.CreateMockContext();
        var mockSet = GetMockCategoryDbSet(categories);
        
        mockContext.Setup(x => x.Categories).Returns(mockSet.Object);
        mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database failure"));

        return mockContext;
    }
    
    public static Mock<ILogger<CategoryHandler>> GetMockLogger() => new();
}