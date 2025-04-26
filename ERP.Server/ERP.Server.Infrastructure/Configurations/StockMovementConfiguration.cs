using ERP.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Server.Infrastructure.Configurations;

internal sealed class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.Property(p => p.NumberOfEntries).HasColumnType("decimal(7,2)");
        builder.Property(p => p.NumberOfOutputs).HasColumnType("decimal(7,2)");
        builder.Property(p => p.Price).HasColumnType("money");
    }
}
