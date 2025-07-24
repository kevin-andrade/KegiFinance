using KegiFin.Api.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Requests;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Seeds;
using KegiFin.Tests.Unit.Helpers.Mocking;
using Microsoft.Extensions.Logging;

namespace KegiFin.Tests.Unit.Api.Handlers.Reports.Get;

public class GetIncomesAndExpensesHandlerTests
{
    [Fact]
    public async Task GetIncomesAndExpensesReportAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var incomesAndExpenses = ReportSeed.GetValidRequestIncomesAndExpenses();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(incomesAndExpenses, x => x.IncomesAndExpenses);
        var mockLogger = QueryMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesAndExpensesRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetIncomesAndExpensesReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Single(response.Data);
        Assert.Equal(request.UserId, response.Data.First().UserId);
        
        var expected = incomesAndExpenses.First();
        var actual = response.Data.First();

        Assert.Equal(expected.UserId, actual.UserId);
        Assert.Equal(expected.Year, actual.Year);
        Assert.Equal(expected.Month, actual.Month);
        Assert.Equal(expected.Incomes, actual.Incomes);
        Assert.Equal(expected.Expenses, actual.Expenses);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(mockLogger, LogLevel.Information, "Incomes and Expenses report successfully loaded");
    }

    [Fact]
    public async Task GetIncomesAndExpensesReportAsync_WithMultiplesReports_ReturnsOrderedList()
    {
        // Arrange
        var incomesAndExpenses = ReportSeed.GetOrderByYearAndMonth();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(incomesAndExpenses, x => x.IncomesAndExpenses);
        var mockLogger = QueryMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesAndExpensesRequest();

        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));

        // Act
        var response = await handler.GetIncomesAndExpensesReportAsync(request);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(incomesAndExpenses.Count, response.Data.Count);
        Assert.All(response.Data, x => Assert.Equal(request.UserId, x.UserId));
        
        var ordered = response
            .Data
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();
        
        Assert.Equal(incomesAndExpenses.Count, ordered.Count);
        Assert.Equal(ordered, response.Data);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(mockLogger, LogLevel.Information, "Incomes and Expenses report successfully loaded");
    }

    [Fact]
    public async Task GetIncomesAndExpensesReportAsync_WhenUserWithoutData_ReturnsEmptyList()
    {
        // Arrange
        var incomesAndExpenses = new List<IncomesAndExpenses>();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(incomesAndExpenses, x => x.IncomesAndExpenses);
        var mockLogger = QueryMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesAndExpensesRequest();

        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetIncomesAndExpensesReportAsync(request);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(mockLogger, LogLevel.Information, "Incomes and Expenses report successfully loaded");
    }

    [Fact]
    public async Task GetIncomesAndExpensesReportAsync_WhenExceptionIsThrown_ReturnsFailureWithNullDataAndErrorMessage()
    {
        // Arrange
        var mockContext = QueryMockHelper.CreateMockDbContextWithException(x => x.IncomesAndExpenses);
        var mockLogger = QueryMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateIncomesAndExpensesRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetIncomesAndExpensesReportAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        Assert.Equal("Error loading Incomes and Expenses", response.Message);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(mockLogger, LogLevel.Error, "Error loading Incomes and Expenses");
    }
}