using KegiFin.Core.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KegiFin.Api.Data.Mappings.Reports;

public class ExpensesByCategoryMapping : IEntityTypeConfiguration<ExpensesByCategory>
{
    public void Configure(EntityTypeBuilder<ExpensesByCategory> builder)
    {
        builder.ToView("vwGetExpensesByCategory");
        builder.HasNoKey();

        builder.Property(x => x.UserId);
        builder.Property(x => x.Category);
        builder.Property(x => x.Year);
        builder.Property(x => x.Expenses);
    }
}