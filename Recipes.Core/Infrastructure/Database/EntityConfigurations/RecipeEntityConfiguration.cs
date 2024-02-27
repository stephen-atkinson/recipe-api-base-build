using System.Text.Json;
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

        builder.OwnsMany(r => r.Ingredients, t =>
        {
            t.HasKey($"{nameof(Recipe)}{nameof(Recipe.Id)}", nameof(Ingredient.ExternalId));
        });
        
        builder.OwnsMany(r => r.Ratings, t =>
        {
            t.HasKey($"{nameof(Recipe)}{nameof(Recipe.Id)}", nameof(Rating.UserId));
        });
    }
}