using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Mocks;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.Delete;

public class DeleteCategoryHandlerTests
{
    private readonly List<Category> _categories = [];
    
    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryExist_ShouldDeleteAndReturnSuccess()
    {
        // Arrange
        _categories.Add(new Category
        {
            Id = 1,
            UserId = 123.ToString(),
            Name = "Old Name",
            Description = "Old Desc"
        });
        
        var mockContext = CategoryDeleteMock.GetSuccessMock(_categories);
        var mockLogger = CategoryDeleteMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidDeleteRequest();
        var existCategory = _categories.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
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
        
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Information, 
            $"Category successfully deleted Id: {response.Data?.Id}");
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryDoesNotExist_ShouldReturnFailureResponse()
    {
        // Arrange
        var categories = new List<Category>();
        var mockContext = CategoryDeleteMock.GetNotFoundMock(categories);
        var mockLogger = CategoryDeleteMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidDeleteRequest();
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
        // Act
        var response = await handler.DeleteCategoryAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Category not found", response.Message);
        
        mockContext.Verify(x => x.Categories.Remove(It.IsAny<Category>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Warning, 
            $"Category not found Id: {request.Id} UserId: {request.UserId}");
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenSaveChangesFail_ShouldReturnFailureResponse()
    {
        // Arrange
        _categories.Add(new Category
        {
            Id = 1,
            UserId = 123.ToString(),
            Name = "Old Name",
            Description = "Old Desc"
        });

        var mockContext = CategoryDeleteMock.GetFailMock(_categories);
        var mockLogger = CategoryDeleteMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidDeleteRequest();
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
        // Act
        var response = await handler.DeleteCategoryAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Error category deleted", response.Message);
        
        mockContext.Verify(x => x.Categories.Remove(It.IsAny<Category>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Error, 
            $"Error deleting category Id: {request.Id} | UserId: {request.UserId}");
    }
}
