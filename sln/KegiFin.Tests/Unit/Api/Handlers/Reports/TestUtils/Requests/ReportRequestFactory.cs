using KegiFin.Core.Requests.Reports;

namespace KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Requests;

public static class ReportRequestFactory
{
    public static GetIncomesAndExpensesRequest CreateIncomesAndExpensesRequest(string? userId = "user-1")
        => new() { UserId = userId ?? string.Empty };
    
    public static GetIncomesByCategoryRequest CreateIncomesByCategoryRequest(string? userId = "user-1")
        => new() { UserId = userId ?? string.Empty };
    
    public static GetExpensesByCategoryRequest CreateExpensesByCategoryRequest(string? userId = "user-1")
        => new() { UserId = userId ?? string.Empty };
    
    public static GetFinancialSummaryRequest CreateFinancialSummaryRequest(string? userId = "user-1")
        => new() { UserId = userId ?? string.Empty };
}