using KegiFin.Api.Handlers;
using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Helpers.Mocking.Db.Crud;
using KegiFin.Tests.Unit.Helpers.Mocking.Logging;
using KegiFin.Tests.Unit.Helpers.Testing;
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

        var mockContext = CrudMockHelper.CreateAsyncMockDbContext(_categories, x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidUpdateRequest(1, 123.ToString());

        var handler = HandlerTestHelper<CategoryHandler>
            .CreateHandler(mockContext, mockLogger,
                (ctx, logger) => new CategoryHandler(ctx, logger));
        
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
        
        HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger,
            LogLevel.Information, $"Category successfully update Id: 1");
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_WhenCategoryNotFound_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = CrudMockHelper.CreateAsyncMockDbContext([], x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidUpdateRequest(1, 123.ToString());
        
        var handler = HandlerTestHelper<CategoryHandler>
            .CreateHandler(mockContext, mockLogger,
                (ctx, logger) => new CategoryHandler(ctx, logger));
        
        // Act
        var response = await handler.UpdateCategoryAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Category not found", response.Message);
        
        mockContext.Verify(x => x.Categories.Update(It.IsAny<Category>()), Times.Never);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger, LogLevel.Warning, 
            $"Category not found Id: {request.Id} UserId: {request.UserId}");
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_WhenSaveChangesFails_ReturnsFailure()
    {
        var data = new List<Category> { new() { Id = 1, Name = "Dev", UserId = "123", Description = "Teste"} };
        var mockContext = CrudMockHelper.CreateAsyncMockDbContextWithSaveFailure(data, x => x.Categories); //Mock context Fail save
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidUpdateRequest();
        
        var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
            mockContext, mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger) );
        
        // Act
        var response = await handler.UpdateCategoryAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Error updating category", response.Message);
        
        mockContext.Verify(x => x.Categories.Update(It.IsAny<Category>()), Times.Once);
        mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger, LogLevel.Error, 
            $"Error updating category userId: {request.UserId} | Name: {request.Name}");
    }
}