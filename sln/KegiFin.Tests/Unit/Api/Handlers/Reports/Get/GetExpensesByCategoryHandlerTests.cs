using KegiFin.Api.Handlers;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Requests;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Seeds;
using KegiFin.Tests.Unit.Helpers.Mocking.Db.Query;
using KegiFin.Tests.Unit.Helpers.Mocking.Logging;
using KegiFin.Tests.Unit.Helpers.Testing;
using Microsoft.Extensions.Logging;

namespace KegiFin.Tests.Unit.Api.Handlers.Reports.Get;

public class GetExpensesByCategoryHandlerTests
{
    [Fact]
    public async Task GetExpensesByCategoryReportAsync_SingleEntryRequest_ReturnsSuccess()
    {
        // Arrange
        var expensesByCategory = ReportSeed.GetExpensesByCategorySingle();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(expensesByCategory, x => x.ExpensesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateExpensesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetExpensesByCategoryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal(request.UserId, response.Data.First().UserId);
        
        var expected = expensesByCategory.First();
        var actual = response.Data.First();
        
        Assert.Equal(expected.UserId, actual.UserId);
        Assert.Equal(expected.Category, actual.Category);
        Assert.Equal(expected.Year, actual.Year);
        Assert.Equal(expected.Expenses, actual.Expenses);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Information,
            "Expenses by category report successfully loaded");
    }
    
    [Fact]
    public async Task GetIncomesByCategoryReportAsync_MultiplesEntriesRequest_ReturnsOrderedList()
    {
        // Arrange
        var expensesByCategory = ReportSeed.GetOrderByYearAndCategoryExpenses();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(expensesByCategory, x => x.ExpensesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateExpensesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger) );
        
        // Act
        var response = await handler.GetExpensesByCategoryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(expensesByCategory.Count, response.Data.Count);
        Assert.All(response.Data, i => Assert.Equal(request.UserId, i.UserId));

        var expectedOrder = expensesByCategory
            .OrderByDescending(x => x.Year)
            .ThenBy(x => x.Category)
            .ToList();

        Assert.Equal(expectedOrder, response.Data);

        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Information,
            "Expenses by category report successfully loaded");
    }
    
    [Fact]
    public async Task GetExpensesByCategoryReportAsync_EmptyDataRequest_ReturnsEmptyList()
    {
        var mockContext = QueryMockHelper.CreateMockDbContextWithData([], x => x.ExpensesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateExpensesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetExpensesByCategoryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Information,
            "Expenses by category report successfully loaded");
    }
    
    [Fact]
    public async Task GetExpensesByCategoryReportAsync_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        var mockContext = QueryMockHelper.CreateMockDbContextWithException(x => x.ExpensesByCategories);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateExpensesByCategoryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetExpensesByCategoryReportAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        
        Assert.Equal("Error loading Expenses by category report", response.Message);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger, LogLevel.Error, "Error loading Expenses by category report");
    }
}