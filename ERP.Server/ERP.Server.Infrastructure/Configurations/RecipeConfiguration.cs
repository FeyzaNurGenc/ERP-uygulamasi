using ERP.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.Server.Infrastructure.Configurations;

internal sealed class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        //Aralarında ilişki var ama Recipe silinirse bile Product silinmesin
        builder.HasOne(p => p.Product).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
