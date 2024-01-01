using Asp.Versioning;
using InnAi.Server.Clients;
using InnAi.Server.Data;
using InnAi.Server.Data.Repositories;
using InnAiServer.Options;
using InnAiServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace InnAi.Server;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string? MongoConnectionString => _configuration.GetConnectionString("MongoDb"); 
    
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddApiVersioning(x =>
        {
            x.DefaultApiVersion = new ApiVersion(1,0);
            x.AssumeDefaultVersionWhenUnspecified = true;
            x.ReportApiVersions = true;
            x.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        });
        
        services.AddMemoryCache();

        ConfigureOptions(services);
        ConfigureCustomServices(services);
        ConfigureSwagger(services);
        ConfigureDatabase(services);
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo {
                Title = "InnAi API",
                Description = "The REST api provides current rain radar images in conjunction with the appropriate inn level.",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Name = "Jakob Bauer",
                    Url = new Uri("https://www.bauer-jakob.de"),
                    Email = "info@bauer-jakob.de"
                }
            });
        });
    }

    private void ConfigureOptions(IServiceCollection services)
    {
        services.Configure<ApiClients>(_configuration.GetSection(nameof(ApiClients)));
    }

    private void ConfigureCustomServices(IServiceCollection services)
    {
        services.AddScoped<IInnAiService, InnAiService>();
        services.AddScoped<IPredictionService, PredictionService>();
        services.AddScoped<IInnLevelClient, InnLevelClient>();
        services.AddScoped<IPrecipitationMapClient, PrecipitationMapClient>();
        services.AddScoped<IInnAiPredictionClient, InnAiPredictionClient>();
    }

    private void ConfigureDatabase(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MainDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IMainDbRepository, MainDbRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // if (env.IsDevelopment())
        // {
        app.UseSwagger();
        app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();
        
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "InnAi API");
        });
    }
}