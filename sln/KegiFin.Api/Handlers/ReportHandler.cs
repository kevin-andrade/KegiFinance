using KegiFin.Api.Data;
using KegiFin.Core.Enums;
using KegiFin.Core.Handlers;
using KegiFin.Core.Models.Reports;
using KegiFin.Core.Requests.Reports;
using KegiFin.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace KegiFin.Api.Handlers;

public class ReportHandler(IAppDbContext context, ILogger<ReportHandler> logger) : IReportHandler
{
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
    {
        try
        {
            var data = await context
                .IncomesAndExpenses
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();
            
            logger.LogInformation("Incomes and Expenses report successfully loaded");
            return new Response<List<IncomesAndExpenses>?>(data);
        }
        catch (Exception e)
        {
            
            logger.LogError(e, "Error loading Incomes and Expenses");
            
            return new Response<List<IncomesAndExpenses>?>(null, "Error loading Incomes and Expenses", 500);
        }
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
    {
        try
        {
            var data = await context
                .IncomesByCategories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();
            
            logger.LogInformation("Incomes by category report successfully loaded");
            return new Response<List<IncomesByCategory>?>(data);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error loading Incomes by category report");
            return new Response<List<IncomesByCategory>?>(null, "Error loading Incomes by category report", 500);
        }
    }

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request)
    {
        try
        {
            var data = await context
                .ExpensesByCategories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();
            
            logger.LogInformation("Expenses by category report successfully loaded");
            return new Response<List<ExpensesByCategory>?>(data);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error loading Expenses by category report");
            return new Response<List<ExpensesByCategory>?>(null, "Error loading Expenses by category report", 500);
        }
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = DateTime.Now.AddDays(1).Date;
        try
        {
            var data = await context
                .Transactions
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId
                            && x.PaidOrReceivedAt >= startDate
                            && x.PaidOrReceivedAt < endDate)
                .GroupBy(x => true)
                .Select(x => new FinancialSummary(
                    request.UserId,
                    x.Where(ty => ty.Type == ETransactionType.Deposit).Sum(t => t.Amount),
                    x.Where(ty => ty.Type == ETransactionType.Withdraw).Sum(t => t.Amount)))
                .FirstOrDefaultAsync();
            
            if (data is null)
            {
                logger.LogWarning("No transactions found for user {UserId} in the current month.", request.UserId);
                var emptyData = new FinancialSummary(request.UserId, 0, 0);
                return new Response<FinancialSummary?>(emptyData, "No transactions found in the current month.");
            }
            
            logger.LogInformation("Financial summary report successfully loaded");
            return new Response<FinancialSummary?>(data);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error loading Financial summary report");
            return new Response<FinancialSummary?>(null, "Error loading Financial summary report", 500);
        }
    }
}