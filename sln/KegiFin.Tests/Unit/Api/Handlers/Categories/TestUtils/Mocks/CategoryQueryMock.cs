using KegiFin.Api.Data;
using KegiFin.Api.Handlers;
using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Helpers.Mocking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Mocks;

public static class CategoryQueryMock
{
    private static Mock<DbSet<Category>> GetMockCategoryDbSet(List<Category> categories)
    {
        var mockSet = DbSetMockHelper.CreateMockDbSet(categories);
        return mockSet;
    }

    public static Mock<IAppDbContext> GetMockForQueries(List<Category> categories)
    {
        var mockContext = CategoryHandlerTestHelper.CreateMockContext();
        var mockSet = GetMockCategoryDbSet(categories);
        
        mockContext.Setup(x => x.Categories).Returns(mockSet.Object);
        
        return mockContext;
    }
    
    public static Mock<IAppDbContext> GetMockForQueriesError()
    {
        var mockSet = new Mock<DbSet<Category>>();

        mockSet.As<IQueryable<Category>>()
            .Setup(m => m.Provider)
            .Throws(new Exception("Database failure on query"));

        var mockContext = CategoryHandlerTestHelper.CreateMockContext();
        mockContext.Setup(x => x.Categories).Returns(mockSet.Object);
        
        return mockContext;
    }
    
    public static Mock<ILogger<CategoryHandler>> GetMockLogger() => new();
}