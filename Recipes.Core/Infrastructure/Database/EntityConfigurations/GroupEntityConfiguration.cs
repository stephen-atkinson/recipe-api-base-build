using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Core.Domain;

namespace Recipes.Core.Infrastructure.Database.EntityConfigurations;

public class GroupEntityConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasMany(m => m.Recipes).WithMany(r => r.Groups);
    }
}