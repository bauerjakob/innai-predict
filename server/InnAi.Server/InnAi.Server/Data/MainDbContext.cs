using InnAi.Server.Data.Entities;
using InnAi.Server.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace InnAi.Server.Data;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<AiModel> AiModels { get; set; }
    public DbSet<AiModelResult> AiModelResults { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AiModelMapping());
        modelBuilder.ApplyConfiguration(new PredictionArchiveMapping());
    }
}