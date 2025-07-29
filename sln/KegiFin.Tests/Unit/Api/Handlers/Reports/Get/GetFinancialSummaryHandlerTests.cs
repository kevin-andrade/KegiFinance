using KegiFin.Api.Handlers;
using KegiFin.Core.Enums;
using KegiFin.Core.Models;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Requests;
using KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Seeds;
using KegiFin.Tests.Unit.Helpers.Mocking.Db.Query;
using KegiFin.Tests.Unit.Helpers.Mocking.Logging;
using KegiFin.Tests.Unit.Helpers.Testing;
using Microsoft.Extensions.Logging;

namespace KegiFin.Tests.Unit.Api.Handlers.Reports.Get;

public class GetFinancialSummaryHandlerTests
{
    private readonly DateTime _startDate = new (DateTime.Now.Year, DateTime.Now.Month, 1);
    private readonly DateTime _endDate = DateTime.Now.AddDays(1).Date;
    
    [Fact]
    public async Task GetFinancialSummaryReportAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var transactions = TransactionSeed.GetTransactions();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(transactions, x => x.Transactions);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateFinancialSummaryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetFinancialSummaryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.UserId, response.Data.UserId);
        
        var expectedIncomes = transactions
            .Where(t => t.UserId == request.UserId &&
                        t.PaidOrReceivedAt >= _startDate &&
                        t.PaidOrReceivedAt < _endDate &&
                        t.Type == ETransactionType.Deposit)
            .Sum(t => t.Amount);

        var expectedExpenses = transactions
            .Where(t => t.UserId == request.UserId &&
                        t.PaidOrReceivedAt >= _startDate &&
                        t.PaidOrReceivedAt < _endDate &&
                        t.Type == ETransactionType.Withdraw)
            .Sum(t => t.Amount);

        Assert.Equal(expectedIncomes, response.Data.Incomes);
        Assert.Equal(expectedExpenses, response.Data.Expenses);
        Assert.Equal(expectedIncomes - expectedExpenses, response.Data.Total);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(mockLogger, 
            LogLevel.Information, "Financial summary report successfully loaded");
    }

    [Fact]
    public async Task GetFinancialSummaryReportAsync_WhenUserNoTransactions_ReturnsFailure()
    {
        // Arrange
        var transactions = new List<Transaction>();
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(transactions, x => x.Transactions);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateFinancialSummaryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, 
                (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetFinancialSummaryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.UserId, response.Data.UserId);
        
        Assert.Equal(0, response.Data.Incomes);
        Assert.Equal(0, response.Data.Expenses);
        Assert.Equal(0, response.Data.Total);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger,
            LogLevel.Warning, $"No transactions found for user {response.Data.UserId} in the current month.");
        Assert.Equal("No transactions found in the current month.", response.Message);
    }

    [Fact]
    public async Task GetFinancialSummaryReportAsync_WhenDifferentUser_ReturnsFailure()
    {
        // Arrange
        var transactions = TransactionSeed.GetTransactionsWithOtherUserOnly(); // userId: user-2
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(transactions, x => x.Transactions);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateFinancialSummaryRequest(); // userId: user-1

        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger));

        // Act
        var response = await handler.GetFinancialSummaryReportAsync(request);

        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.UserId, response.Data.UserId);
        
        Assert.True(response.IsSuccess);
        Assert.Equal(0, response.Data.Incomes);
        Assert.Equal(0, response.Data.Expenses);
        Assert.Equal(0, response.Data.Total);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger,
            LogLevel.Warning, $"No transactions found for user {response.Data.UserId} in the current month.");
        Assert.Equal("No transactions found in the current month.", response.Message);
    }
    
    [Fact]
    public async Task? GetFinancialSummaryReportAsync_WhenUserGetOldTransactions_ReturnsFailure()
    {
        // Arrange
        var transactions = TransactionSeed.GetOldTransactionsForUser(); // Old Transactions
        var mockContext = QueryMockHelper.CreateMockDbContextWithData(transactions, x => x.Transactions);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateFinancialSummaryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetFinancialSummaryReportAsync(request);
        
        // Assert
        Assert.True(response.IsSuccess);
        Assert.NotNull(response.Data);
        Assert.Equal(request.UserId, response.Data.UserId);
        
        Assert.True(response.IsSuccess);
        Assert.Equal(0, response.Data.Incomes);
        Assert.Equal(0, response.Data.Expenses);
        Assert.Equal(0, response.Data.Total);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger,
            LogLevel.Warning, $"No transactions found for user {response.Data.UserId} in the current month.");
        Assert.Equal("No transactions found in the current month.", response.Message);
    }

    [Fact]
    public async Task GetFinancialSummaryReportAsync_WhenExceptionIsThrown_ReturnsFailureWithNullDataAndErrorMessage()
    {
        // Arrange
        var mockContext = QueryMockHelper.CreateMockDbContextWithException(x => x.Transactions);
        var mockLogger = LoggerMockHelper.GetMockLogger<ReportHandler>();
        var request = ReportRequestFactory.CreateFinancialSummaryRequest();
        
        var handler = HandlerTestHelper<ReportHandler>
            .CreateHandler(mockContext, mockLogger, (context, logger) => new ReportHandler(context, logger));
        
        // Act
        var response = await handler.GetFinancialSummaryReportAsync(request);
        
        // Assert
        Assert.False(response.IsSuccess);
        Assert.Null(response.Data);
        
        HandlerTestHelper<ReportHandler>.VerifyLog(
            mockLogger,
            LogLevel.Error, "Error loading Financial summary report");
        Assert.Equal("Error loading Financial summary report", response.Message);
    }
}