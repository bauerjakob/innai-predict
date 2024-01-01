using InnAi.Server;
using InnAi.Server.Data;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var host = CreateHostBuilder(args).Build();
MigrateDatabase(host);
host.Run();


static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging => logging.AddConsole())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
        
        
static void MigrateDatabase(IHost host)
{
    using var scope = host.Services.GetService<IServiceScopeFactory>()?.CreateScope();
    
    if (scope == null) return;
    
    using var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
    if (db.Database.GetPendingMigrations().Any())
    {
        db.Database.Migrate();
    }
}