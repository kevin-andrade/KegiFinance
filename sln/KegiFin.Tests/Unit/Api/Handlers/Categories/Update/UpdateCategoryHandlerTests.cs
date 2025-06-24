using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Mocks;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.Update;

public class UpdateCategoryHandlerTests
{
    private readonly List<Category> _categories = [];
    
    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryExists_ShouldUpdateAndReturnSuccess()
    {
        // Arrange
        _categories.Add(new Category
        {
            Id = 1,
            UserId = 123.ToString(),
            Name = "Old Name",
            Description = "Old Desc"
        });

        var mockContext = CategoryUpdateMock.GetSuccessMock(_categories);
        var mockLogger = CategoryUpdateMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidUpdateRequest(1, 123.ToString());

        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
        // Act
        var response = await handler.UpdateCategoryAsync(request);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.Name, response.Data?.Name);
        Assert.Equal(request.Description, response.Data?.Description);
        Assert.Equal("Category successfully update", response.Message);

        var updatedCategory = _categories.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);
        Assert.NotNull(updatedCategory);
        Assert.Equal(request.Name, updatedCategory.Name);
        
        mockContext.Verify(x => x.Categories.Update(It.IsAny<Category>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Information, $"Category successfully update Id: 1");
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryDoesNotExist_ShouldReturnFailureResponse()
    {
        // Arrange
        var categories = new List<Category>(); //List empty
        var mockContext = CategoryUpdateMock.GetNotFoundMock(categories); //Mock context with Not Found
        var mockLogger = CategoryUpdateMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidUpdateRequest(1, 123.ToString());
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
        // Act
        var response = await handler.UpdateCategoryAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Category not found", response.Message);
        
        mockContext.Verify(x => x.Categories.Update(It.IsAny<Category>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Warning, 
            $"Category not found Id: {request.Id} UserId: {request.UserId}");
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_WhenSaveChangesFails_ShouldReturnFailureResponse()
    {
        // Arrange
        _categories.Add(new Category
        {
            Id = 1,
            UserId = 123.ToString(),
            Name = "Old Name",
            Description = "Old Desc"
        });
        
        var mockContext = CategoryUpdateMock.GetFailMock(_categories); //Mock context Fail save
        var mockLogger = CategoryUpdateMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidUpdateRequest();
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
        // Act
        var response = await handler.UpdateCategoryAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Error updating category", response.Message);
        
        mockContext.Verify(x => x.Categories.Update(It.IsAny<Category>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Error, 
            $"Error updating category userId: {request.UserId} | Name: {request.Name}");
    }
}