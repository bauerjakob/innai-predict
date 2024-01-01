using System.Text.Json;
using System.Text.Json.Serialization;
using InnAi.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnAi.Server.Data.Mappings;

public class PredictionArchiveMapping : IEntityTypeConfiguration<AiModelResult>
{
    public void Configure(EntityTypeBuilder<AiModelResult> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new {
            x.DateTime, x.AiModelId
        }).IsUnique();

        builder.Property(x => x.DateTime).IsRequired();
        
        var jsonOptions = new JsonSerializerOptions();
        builder.Property(x => x.PredictionValues)
            .HasConversion(
                x => JsonSerializer.Serialize(x, jsonOptions),
                x => JsonSerializer.Deserialize<double[]>(x, jsonOptions)!);
        
        builder.Property(x => x.ActualValues)
            .HasConversion(
                x => JsonSerializer.Serialize(x, jsonOptions),
                x => JsonSerializer.Deserialize<double[]>(x, jsonOptions)!);
    }
}