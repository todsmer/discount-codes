using Discounts.Application.Entities;
using Discounts.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Discounts.Infrastructure.Database.Configuration;

internal class DiscountCodeConfiguration : IEntityTypeConfiguration<DiscountCode>
{
    public void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Code)
            .HasMaxLength(DiscountCodeConstants.MaxCodeLength)
            .IsRequired();

        builder.Property(x => x.IsUsed)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}
