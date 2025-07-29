using KegiFin.Api.Handlers;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Helpers.Mocking;
using KegiFin.Tests.Unit.Helpers.Mocking.Db.Query;
using KegiFin.Tests.Unit.Helpers.Mocking.Logging;
using KegiFin.Tests.Unit.Helpers.Testing;
using Microsoft.Extensions.Logging;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.Get;

public class CategoryQueryHandlerTests
{
    #region Get By Id

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryExists_ReturnSuccess()
    {
        // Arrange
        var categories = CategoryRequestFactory.GetSampleCategories();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(categories, x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidCategoryByIdRequest();

        var handler = HandlerTestHelper<CategoryHandler>
            .CreateHandler(mockContext, mockLogger,
                (ctx, logger) => new CategoryHandler(ctx, logger));

        // Act
        var response = await handler.GetCategoryByIdAsync(request);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.Id, response.Data?.Id);
        Assert.Equal(request.UserId, response.Data?.UserId);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryNotFound_ReturnsFailure()
    {
        var categories = CategoryRequestFactory.GetEmptyCategoryList();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(categories, x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidCategoryByIdRequest();

        var handler = HandlerTestHelper<CategoryHandler>
            .CreateHandler(mockContext, mockLogger, 
                (ctx, logger) => new CategoryHandler(ctx, logger));

        var response = await handler.GetCategoryByIdAsync(request);

        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Category not found", response.Message);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryByIdError_ReturnsFailure()
    {
        // Arrange
        var mockContext = QueryMockHelper.CreateMockDbContextWithException(x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidCategoryByIdRequest(99, 999.ToString());

        var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
            mockContext, mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger));
    
        // Act
        var response = await handler.GetCategoryByIdAsync(request);
    
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Error load category", response.Message);
    
        HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger, LogLevel.Error,
            $"Error load category by Id: {request.Id} | UserId: {request.UserId}");
    }
    
    #endregion
    
    #region Get All
    
    [Fact]
    public async Task GetAllCategoriesAsync_WhenCategoriesExists_ReturnsSuccess()
    {
        // Arrange
        var categories = CategoryRequestFactory.GetSampleCategories();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(categories, x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidAllCategoriesRequest();
        
        var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
            mockContext, mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger));
        
        // Act
        var response = await handler.GetAllCategoriesAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(3, response.Data?.Count);
        Assert.Equal(3, response.TotalCount);
    
        Assert.Equal(request.PageNumber, response.CurrentPage);
        Assert.Equal(request.PageSize, response.PageSize);
        Assert.Equal(request.UserId, response.Data?.First().UserId);
        Assert.True(response.Data?.SequenceEqual(response.Data.OrderBy(x => x.Name)));
    }
    
    [Fact]
     public async Task GetAllCategoriesAsync_WhenCategoriesNotFound_ReturnsEmptyList()
     {
         // Arrange
         var emptyCategories = CategoryRequestFactory.GetEmptyCategoryList();
         var mockContext = QueryMockHelper.CreateMockDbContextWithData(emptyCategories, x => x.Categories);
         var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
         var request = CategoryRequestFactory.GetValidAllCategoriesRequest();
    
         var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
             mockContext, mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger));
    
         // Act
         var response = await handler.GetAllCategoriesAsync(request);
    
         // Assert
         Assert.True(response.IsSuccess);
         Assert.NotNull(response.Data);
         Assert.Empty(response.Data);
         Assert.Equal(0, response.TotalCount);
    
         Assert.Equal(request.PageNumber, response.CurrentPage);
         Assert.Equal(request.PageSize, response.PageSize);
     }
    
    [Fact]
    public async Task GetAllCategoriesAsync_WhenCategoriesError_ReturnsFailure()
    {
        // Arrange
        var mockContext = QueryMockHelper.CreateMockDbContextWithException(x => x.Categories);
        var mockLogger = LoggerMockHelper.GetMockLogger<CategoryHandler>();
        var request = CategoryRequestFactory.GetValidAllCategoriesRequest();
    
        var handler = HandlerTestHelper<CategoryHandler>.CreateHandler(
            mockContext, mockLogger, (ctx, logger) => new CategoryHandler(ctx, logger));
    
        // Act
        var response = await handler.GetAllCategoriesAsync(request);
    
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal(0, response.TotalCount);
        Assert.Equal("Error load all categories", response.Message);
    
        Assert.Equal(0, response.CurrentPage);
        Assert.Equal(request.PageSize, response.PageSize);
        
        HandlerTestHelper<CategoryHandler>.VerifyLog(mockLogger, LogLevel.Error,
            $"Error load all categories userId: {request.UserId}");
    }
    
    #endregion
}