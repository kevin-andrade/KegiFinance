using KegiFin.Core.Models;
using KegiFin.Core.Models.Reports;
using Microsoft.EntityFrameworkCore;

namespace KegiFin.Api.Data;

public interface IAppDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<ExpensesByCategory> ExpensesByCategories { get; set; }
    DbSet<IncomesByCategory> IncomesByCategories { get; set; }
    DbSet<IncomesAndExpenses> IncomesAndExpenses { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}