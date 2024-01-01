using InnAi.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnAi.Server.Data.Mappings;

public class AiModelMapping : IEntityTypeConfiguration<AiModel>
{
    public void Configure(EntityTypeBuilder<AiModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ExternalId).IsUnique();

        builder
            .HasMany(x => x.Results)
            .WithOne(x => x.AiModel);
    }
}