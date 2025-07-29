using KegiFin.Api.Handlers;
using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Helpers.Mocking.Db.Crud;
using KegiFin.Tests.Unit.Helpers.Mocking.Logging;
using KegiFin.Tests.Unit.Helpers.Testing;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.Delete;

public class DeleteCategoryHandlerTests
{
    private readonly List<Category> _categories = [];
    
    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryExists_ShouldDeleteAndReturnSuccess()
    {
        // Arrange
        _categories.Add(new Category
        {
            Id = 1,
            UserId = 123.ToString(),
            Name = "Old Name",
            Description = "Old Desc"
        });
        
        var mockContext = CrudMockHelper.CreateAsyncMockDbContext(_categories, x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidDeleteRequest();
        var existCategory = _categories.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);
        
        var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
            mockContext,  mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger));
        
        Assert.NotNull(existCategory);
        // Act
        var response = await handler.DeleteCategoryAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.Id, response.Data?.Id);
        Assert.Equal(request.UserId, response.Data?.UserId);
        Assert.Equal("Category successfully deleted", response.Message);
        
        var deletedCategory = _categories.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);
        Assert.Null(deletedCategory);
        
        mockContext.Verify(x => x.Categories.Remove(It.IsAny<Category>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger, LogLevel.Information, 
            $"Category successfully deleted Id: {response.Data?.Id}");
    }

     [Fact]
     public async Task DeleteCategoryAsync_WhenCategoryNotFound_ReturnsFailure()
     {
         // Arrange
         var mockContext = CrudMockHelper.CreateAsyncMockDbContext([], x => x.Categories);
         var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
         var request = CategoryRequestFactory.GetValidDeleteRequest();
         
         var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
             mockContext,  mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger));
         
         // Act
         var response = await handler.DeleteCategoryAsync(request);
         
         // Assert
         Assert.False(response.IsSuccess);
         Assert.Null(response.Data);
         Assert.Equal("Category not found", response.Message);
         
         mockContext.Verify(x => x.Categories.Remove(It.IsAny<Category>()), Times.Never);
         mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
         
         HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger, LogLevel.Warning, 
             $"Category not found Id: {request.Id} UserId: {request.UserId}");
     }

     [Fact]
     public async Task DeleteCategoryAsync_WhenSaveChangesFail_ReturnsFailure()
     {
         // Arrange
         var data = new List<Category> { new() { Id = 1, Name = "Dev", UserId = "123", Description = "Teste"} };
         var mockContext = CrudMockHelper.CreateAsyncMockDbContextWithSaveFailure(data, x => x.Categories);
         var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
         var request = CategoryRequestFactory.GetValidDeleteRequest();
         
         var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
             mockContext,  mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger));
         
         // Act
         var response = await handler.DeleteCategoryAsync(request);
         
         // Assert
         Assert.False(response.IsSuccess);
         Assert.Null(response.Data);
         Assert.Equal("Error category deleted", response.Message);
         
         mockContext.Verify(x => x.Categories.Remove(It.IsAny<Category>()), Times.Once);
         mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
         HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger, LogLevel.Error, 
             $"Error deleting category Id: {request.Id} | UserId: {request.UserId}");
     }
}