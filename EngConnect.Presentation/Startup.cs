using CrystalQuartz.AspNetCore;
using EngConnect.Application.DependencyInjection.Extensions;
using EngConnect.BuildingBlock.Presentation.DependencyInjection.Extensions;
using EngConnect.BuildingBlock.Presentation.Middlewares;
using EngConnect.Domain.Settings;
using EngConnect.Infrastructure.DependencyInjection.Extensions;
using Quartz;

namespace EngConnect.Presentation;

public static class Startup
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.ConfigureSwagger("EngConnect", "v1", new[] { AssemblyReference.Assembly, Application.AssemblyReference.Assembly });
        builder.ConfigureAuthentication();
        builder.ConfigureAuthorization();
        builder.ConfigureAppSettings();
        builder.ConfigureControllers();
    }
    
    private static void ConfigureControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(_ =>
        {
            // Register custom model binders here
        });
    }

    private static void ConfigureAppSettings(this WebApplicationBuilder builder)
    {
        // Register AppInfo as a singleton service for health check
        builder.Services.AddSingleton<AppInfo>();

        // Bind AppSettings from configuration
        _ = builder.Configuration.GetSection(AppSettings.Section).Get<AppSettings>() ??
            throw new Exception("AppSettings are not configured");
        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.Section));
    }

    /// <summary>
    ///     Configures rate limiting for the application
    /// </summary>
    public static void ConfigureRateLimiting(this WebApplicationBuilder builder)
    {
    }

    /// <summary>
    ///     Configures the application request pipeline
    /// </summary>
    public static void Configure(this WebApplication app)
    {
        app.UseErrorHandling();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerApiReference("EngConnect", "v1");
            app.UseRedoc("EngConnect API Documentation", "/openapi/v1.json", "docs/api");
        }
        else
        {
            app.UseHsts();
        }

        // Start Quartz scheduler for background jobs
        // var scheduler = app.Services.GetRequiredService<ISchedulerFactory>().GetScheduler().Result;
        // app.UseCrystalQuartz(() => scheduler);

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        // Configure static files
        app.UseStaticFiles();
        
        app.MapControllers();
    }
}