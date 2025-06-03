using KegiFin.Core.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KegiFin.Api.Data.Mappings.Reports;

public class IncomesByCategoryMapping : IEntityTypeConfiguration<IncomesByCategory>
{
    public void Configure(EntityTypeBuilder<IncomesByCategory> builder)
    {
        builder.ToView("vwGetIncomesByCategory");
        builder.HasNoKey();

        builder.Property(x => x.UserId);
        builder.Property(x => x.Category);
        builder.Property(x => x.Year);
        builder.Property(x => x.Incomes);
    }
}