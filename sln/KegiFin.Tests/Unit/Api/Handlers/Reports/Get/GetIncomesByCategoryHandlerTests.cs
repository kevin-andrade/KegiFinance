using KegiFin.Api.Handlers;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Requests;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Seeds;
using KegiFin.Tests.Unit.Helpers.Mocking.Db.Query;
using KegiFin.Tests.Unit.Helpers.Mocking.Logging;
using KegiFin.Tests.Unit.Helpers.Testing;
using Microsoft.Extensions.Logging;

namespace KegiFin.Tests.Unit.Api.Handlers.Reports.Get;

public class GetIncomesByCategoryHandlerTests
{
    [Fact]
    public async Task GetIncomesByCategoryReportAsync_SingleEntryRequest_ReturnsSuccess()
    {
        // Arrange
        var incomesByCategory = ReportSeed.GetIncomesByCategorySingle();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(incomesByCategory, x => x.IncomesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));

        // Act
        var response = await handler.GetIncomesByCategoryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal(request.UserId, response.Data.First().UserId);
        
        var expected = incomesByCategory.First();
        var actual = response.Data.First();

        Assert.Equal(expected.UserId, actual.UserId);
        Assert.Equal(expected.Category, actual.Category);
        Assert.Equal(expected.Year, actual.Year);
        Assert.Equal(expected.Incomes, actual.Incomes);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Information,
            "Incomes by category report successfully loaded");
    }
    
    [Fact]
    public async Task GetIncomesByCategoryReportAsync_MultiplesEntriesRequest_ReturnsOrderedList()
    {
        // Arrange
        var incomesByCategory = ReportSeed.GetOrderByYearAndCategoryIncomes();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(incomesByCategory, x => x.IncomesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger) );
        
        // Act
        var response = await handler.GetIncomesByCategoryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(incomesByCategory.Count, response.Data.Count);
        Assert.All(response.Data, i => Assert.Equal(request.UserId, i.UserId));

        var expectedOrder = incomesByCategory
            .OrderByDescending(x => x.Year)
            .ThenBy(x => x.Category)
            .ToList();

        Assert.Equal(expectedOrder, response.Data);

        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Information,
            "Incomes by category report successfully loaded");
    }

    [Fact]
    public async Task GetIncomesByCategoryReportAsync_EmptyDataRequest_ReturnsEmptyList()
    {
        var mockContext = QueryMockHelper.CreateMockDbContextWithData([], x => x.IncomesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetIncomesByCategoryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Information,
            "Incomes by category report successfully loaded");
    }
    
    [Fact]
    public async Task GetIncomesByCategoryReportAsync_ExceptionThrown_ReturnsFailureWithMessage()
    {
        // Arrange
        var mockContext = QueryMockHelper.CreateMockDbContextWithException(x => x.IncomesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetIncomesByCategoryReportAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        
        Assert.Equal("Error loading Incomes by category report", response.Message);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Error, "Error loading Incomes by category report");
    }
}