using KegiFin.Core.Models.Reports;

namespace KegiFin.Tests.Unit.Api.Handlers.Reports.TestUtils.Seeds;

public static class ReportSeed
{
    public static List<IncomesAndExpenses> GetValidRequestIncomesAndExpenses(string userId = "user-1")
        => [new(userId, 1, 2024, 2000m, 1000m)];
    
    public static List<IncomesAndExpenses> GetOrderByYearAndMonth(string userId = "user-1")
        =>
        [
            new(userId, 1, 2024, 2000m, 1000m),
            new(userId, 2, 2024, 3000m, 2000m),
            new(userId, 3, 2024, 4000m, 3000m),
            new(userId, 1, 2025, 2000m, 1000m),
            new(userId, 2, 2025, 3000m, 2000m)
        ];
    
    public static List<IncomesByCategory> GetOrderByYearAndCategoryIncomes(string userId = "user-1")
        => 
        [
            new(userId, "Game", 2024, 2000m),
            new(userId, "Food", 2024, 3000m),
            new(userId, "Food", 2025, 4000m),
            new(userId, "Game", 2025, 5000m),
            new(userId, "Tech", 2025, 6000m)
        ];
    
    public static List<ExpensesByCategory> GetOrderByYearAndCategoryExpenses(string userId = "user-1")
        => 
        [
            new(userId, "Game", 2024, 2000m),
            new(userId, "Food", 2024, 3000m),
            new(userId, "Food", 2025, 4000m),
            new(userId, "Game", 2025, 5000m),
            new(userId, "Tech", 2025, 6000m)
        ];
    
    public static List<IncomesByCategory> GetIncomesByCategorySingle(string userId = "user-1")
        => [new(userId, "Game", 2024, 2000m)];
    
    public static List<ExpensesByCategory> GetExpensesByCategorySingle(string userId = "user-1")
        => [new(userId, "Game", 2024, 2000m)];
}