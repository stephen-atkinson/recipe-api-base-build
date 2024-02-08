using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Core.Domain;

namespace Recipes.Core.Infrastructure.Database.EntityConfigurations;

public class IngredientEntityConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Cost).HasPrecision(18, 2);
    }
}