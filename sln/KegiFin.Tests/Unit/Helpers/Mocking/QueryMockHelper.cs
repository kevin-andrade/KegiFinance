using System.Linq.Expressions;
using KegiFin.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Helpers.Mocking;

public static class QueryMockHelper
{
    public static Mock<IAppDbContext> CreateMockDbContextWithData<T>(
        List<T> data,
        Expression<Func<IAppDbContext, DbSet<T>>> dbSelector
    ) where T : class
    {
        var mockSet = DbSetMockHelper.CreateMockDbSet(data);
        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(dbSelector).Returns(mockSet.Object);
        return mockContext;
    }
    
    public static Mock<IAppDbContext> CreateMockDbContextWithException<T>(
        Expression<Func<IAppDbContext, DbSet<T>>> dbSelector
        ) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();
        
        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Throws(new Exception("Database failure on query"));
        
        var mockContext = new Mock<IAppDbContext>();
        mockContext.Setup(dbSelector).Returns(mockSet.Object);
        
        return mockContext;
    }
    
    public static Mock<ILogger<TEntity>> GetMockLogger<TEntity>() => new();
}