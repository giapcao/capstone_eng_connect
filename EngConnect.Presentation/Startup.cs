using CrystalQuartz.AspNetCore;
using EngConnect.Application.DependencyInjection.Extensions;
using EngConnect.BuildingBlock.Presentation.DependencyInjection.Extensions;
using EngConnect.BuildingBlock.Presentation.Middlewares;
using EngConnect.Domain.Settings;
using EngConnect.Infrastructure.DependencyInjection.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using EngConnect.Presentation.Hubs;
using Quartz;

namespace EngConnect.Presentation;

public static class Startup
{
    public static void Configure(this WebApplicationBuilder builder)
    {

        
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.ConfigureSwagger("EngConnect", "v1", new[] { AssemblyReference.Assembly, Application.AssemblyReference.Assembly });
        //Implement temp add cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials(); // nếu dùng cookie / auth
                });
        });
        
        
        builder.ConfigureHeaders();
        
        builder.ConfigureAuthentication();
        builder.AddGoogleAuthentication();
        builder.ConfigureAuthorization();
        builder.ConfigureAppSettings();
        builder.ConfigureControllers();
        
        // Add SignalR
        builder.Services.AddSignalR();
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

    //260308: Configure forwarded headers to support reverse proxy scenarios (e.g., when deployed behind Nginx or Apache)
    public static void ConfigureHeaders(this WebApplicationBuilder builder)
    {
        var appSettings = builder.Configuration.GetSection(AppSettings.Section).Get<AppSettings>() ??
                          throw new Exception("AppSettings are not configured");

        if (appSettings.EnableForwardedHeaders)
        {
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;

                //Only trust the first proxy
                options.ForwardLimit = 1;

                //Clear both to trust any proxy
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }
    }
    
    /// <summary>
    ///     Configures the application request pipeline
    /// </summary>
    public static void Configure(this WebApplication app)
    {
        app.UseErrorHandling();
        var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
        
        if (appSettings.EnableForwardedHeaders)
        {
            app.UseForwardedHeaders();
        }
        if (appSettings.EnableHttpsRedirection)
        {
            app.UseHttpsRedirection();
        }
        
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
        //Implement temp cors
        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseAuthorization();
        
        // Configure static files
        app.UseStaticFiles();
        
        app.MapControllers();
        
        // Map SignalR Hub
        app.MapHub<VideoCallHub>("/hubs/video-call");
    }
}