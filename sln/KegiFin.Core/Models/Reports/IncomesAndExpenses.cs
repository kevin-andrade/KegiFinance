namespace KegiFin.Core.Models.Reports;

public record IncomesAndExpenses(string UserId, int Month, int Year, decimal Incomes, decimal Expenses);