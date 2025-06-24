using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Helpers;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Mocks;
using KegiFin.Tests.Unit.Api.Handlers.Categories.TestUtils.Seed;
using Microsoft.Extensions.Logging;

namespace KegiFin.Tests.Unit.Api.Handlers.Categories.Get;

public class CategoryQueryHandlerTests
{
    #region Get By Id

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryExist_ShouldGetAndReturnSuccess()
    {
        // Arrange
        var categories = CategorySeed.GetCategories();
        var mockContext = CategoryQueryMock.GetMockForQueries(categories);
        var mockLogger = CategoryQueryMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidCategoryByIdRequest();

        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);

        // Act
        var response = await handler.GetCategoryByIdAsync(request);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.Id, response.Data?.Id);
        Assert.Equal(request.UserId, response.Data?.UserId);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryDoesNotExist_ShouldReturnFailureResponse()
    {
        var categories = CategorySeed.GetCategoriesEmpty();
        var mockContext = CategoryQueryMock.GetMockForQueries(categories);
        var mockLogger = CategoryQueryMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidCategoryByIdRequest();

        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);

        var response = await handler.GetCategoryByIdAsync(request);

        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Category not found", response.Message);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryByIdError_ShouldReturnFailureResponse()
    {
        var mockContext = CategoryQueryMock.GetMockForQueriesError();
        var mockLogger = CategoryQueryMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidCategoryByIdRequest(99, 999.ToString());

        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);

        var response = await handler.GetCategoryByIdAsync(request);

        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Error load category", response.Message);

        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Error,
            $"Error load category by Id: {request.Id} | UserId: {request.UserId}");
    }
    
    #endregion

    #region Get All

    [Fact]
    public async Task GetAllCategoriesAsync_WhenCategoriesExist_ShouldGetAllAndReturnSuccess()
    {
        // Arrange
        var categories = CategorySeed.GetCategories();
        var mockContext = CategoryQueryMock.GetMockForQueries(categories);
        var mockLogger = CategoryQueryMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidAllCategoriesRequest();
        
        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);
        
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
    public async Task GetAllCategoriesAsync_WhenCategoriesDoesNotExist_ShouldReturnFailureResponse()
    {
        // Arrange
        var emptyCategories = CategorySeed.GetCategoriesEmpty();
        var mockContext = CategoryQueryMock.GetMockForQueries(emptyCategories);
        var mockLogger = CategoryQueryMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidAllCategoriesRequest();

        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);

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
    public async Task GetAllCategoriesAsync_WhenCategoriesError_ShouldReturnFailureResponse()
    {
        // Arrange
        var mockContext = CategoryQueryMock.GetMockForQueriesError();
        var mockLogger = CategoryQueryMock.GetMockLogger();
        var request = CategoryHandlerTestHelper.GetValidAllCategoriesRequest();

        var handler = CategoryHandlerTestHelper.CreateHandler(mockContext, mockLogger);

        // Act
        var response = await handler.GetAllCategoriesAsync(request);

        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal(0, response.TotalCount);
        Assert.Equal("Error load all categories", response.Message);

        Assert.Equal(0, response.CurrentPage);
        Assert.Equal(request.PageSize, response.PageSize);
        
        CategoryHandlerTestHelper.VerifyLog(mockLogger, LogLevel.Error,
            $"Error load all categories userId: {request.UserId}");
    }

    #endregion
}