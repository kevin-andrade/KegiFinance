using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Mocks;
using Microsoft.Extensions.Logging;
using Moq;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.Create;

public class CreateCategoryHandlerTests
{
    private readonly List<Category> _categories = [];
    
    [Fact]
    public async Task CreateCategoryAsync_WhenSavingCategory_ShouldReturnSuccess()
    {
        // Arrange
        var mockContext = CategoryCreateMock.GetSuccessMock(_categories);
        var mockLogger = CategoryCreateMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidCreateRequest();
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
        // Act
        var response = await handler.CreateCategoryAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.Single(_categories);
        
        Assert.NotNull(response.Data);
        Assert.Equal(request.UserId, response.Data?.UserId);
        Assert.Equal(request.Name, response.Data?.Name);
        Assert.Equal(request.Description, response.Data?.Description);
        Assert.Equal("Category created successfully", response.Message);
        
        Assert.NotNull(_categories[0]);
        Assert.Equal(request.UserId, _categories[0].UserId);
        Assert.Equal(request.Name, _categories[0].Name);
        Assert.Equal(request.Description, _categories[0].Description);
        
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Information, $"Category created successfully Id: {_categories[0].Id}");
    }
    
    [Fact]
    public async Task CreateCategoryAsync_WhenSaveChangesFail_ShouldReturnFailureResponse()
    {
        // Arrange
        var mockContext = CategoryCreateMock.GetFailMock(_categories);
        var mockLogger = CategoryCreateMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidCreateRequest();
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
        //Act
        var response = await handler.CreateCategoryAsync(request);

        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Error creating category", response.Message);
        
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Error, $"Error creating category Id: {request.UserId} | Name: {request.Name}");
    }
}