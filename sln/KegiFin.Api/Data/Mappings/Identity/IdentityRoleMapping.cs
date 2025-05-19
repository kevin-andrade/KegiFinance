using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KegiFin.Api.Data.Mappings.Identity;

public class IdentityRoleMapping : IEntityTypeConfiguration<IdentityRole<long>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<long>> builder)
    {
        builder.ToTable("IdentityRole");
        builder.HasKey(r => r.Id);
        
        builder.HasIndex(r => r.NormalizedName).IsUnique();
        
        builder.Property(r => r.Name).IsRequired().HasMaxLength(256);
        builder.Property(r => r.NormalizedName).IsRequired().HasMaxLength(256);
        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
    }
}