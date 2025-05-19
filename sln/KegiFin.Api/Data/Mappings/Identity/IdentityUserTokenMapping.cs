using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KegiFin.Api.Data.Mappings.Identity;

public class IdentityUserTokenMapping : IEntityTypeConfiguration<IdentityUserToken<long>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<long>> builder)
    {
        builder.ToTable("IdentityUserToken");
        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
        
        builder.Property(ut => ut.UserId).IsRequired();
        builder.Property(ut => ut.LoginProvider).IsRequired().HasMaxLength(128);
        builder.Property(ut => ut.Name).IsRequired().HasMaxLength(128);
    }
}