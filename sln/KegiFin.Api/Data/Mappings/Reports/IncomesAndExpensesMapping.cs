using KegiFin.Core.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KegiFin.Api.Data.Mappings.Reports;

public class IncomesAndExpensesMapping : IEntityTypeConfiguration<IncomesAndExpenses>
{
    public void Configure(EntityTypeBuilder<IncomesAndExpenses> builder)
    {
        builder.ToView("vwGetIncomesAndExpenses");
        builder.HasNoKey();
        
        builder.Property(x => x.UserId);
        builder.Property(x => x.Month);
        builder.Property(x => x.Year);
        builder.Property(x => x.Incomes);
        builder.Property(x => x.Expenses);
    }
}