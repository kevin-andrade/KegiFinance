using System.Net.Http.Json;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Core.Requests.Reports;
using KegiFin.Core.Responses;

namespace KegiFin.Web.Handlers;

public class ReportHandler(IHttpClientFactory httpClientFactory) : IReportHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
        => await _client.GetFromJsonAsync<Response<List<IncomesAndExpenses>?>>($"{Configuration.ReportsBaseUrl}/incomes-and-expenses")
            ?? new Response<List<IncomesAndExpenses>?>(null, "Error getting incomes and expenses report", 400);

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
        => await _client.GetFromJsonAsync<Response<List<IncomesByCategory>?>>($"{Configuration.ReportsBaseUrl}/incomes-by-category")
            ?? new Response<List<IncomesByCategory>?>(null, "Error getting incomes by category report", 400);

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request)
        => await _client.GetFromJsonAsync<Response<List<ExpensesByCategory>?>>($"{Configuration.ReportsBaseUrl}/expenses-by-category")
            ?? new Response<List<ExpensesByCategory>?>(null, "Error getting expenses by category report", 400);

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
        => await _client.GetFromJsonAsync<Response<FinancialSummary?>>($"{Configuration.ReportsBaseUrl}/financial-summary")
            ?? new Response<FinancialSummary?>(null, "Error getting financial summary report", 400);
}