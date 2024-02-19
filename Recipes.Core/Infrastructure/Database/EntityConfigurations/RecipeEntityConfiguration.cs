using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Core.Domain;

namespace Recipes.Core.Infrastructure.Database.EntityConfigurations;

public class RecipeEntityConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired();

        builder.HasMany(r => r.Ingredients).WithOne(i => i.Recipe);

        builder.ToTable("Ooga");
    }
}