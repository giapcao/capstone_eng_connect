using System.Reflection;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Infrastructure.ApiService;
using EngConnect.BuildingBlock.Infrastructure.FileStorage;
using EngConnect.BuildingBlock.Infrastructure.MessageBus;
using EngConnect.BuildingBlock.Infrastructure.Quartz;
using EngConnect.BuildingBlock.Infrastructure.Redis;
using EngConnect.BuildingBlock.Infrastructure.Security.Encryption;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using StackExchange.Redis;

namespace EngConnect.BuildingBlock.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///     Adds Redis to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <exception cref="Exception"></exception>
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(RedisSettings.Section).Get<RedisSettings>() ??
            throw new Exception("RedisSettings are not configured");
        services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.Section));
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            return ConnectionMultiplexer.Connect(
                $"{redisSettings.EndPoints}:{redisSettings.Port},user={redisSettings.User},password={redisSettings.Password}");
        });
        services.AddSingleton<IRedisService, RedisService>();
    }

    /// <summary>
    ///     Adds MassTransit and related services to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="assembly">The assembly containing the consumers to register</param>
    /// <param name="configureBus"></param>
    /// <exception cref="Exception"></exception>
    public static void AddMessageBus(this IServiceCollection services, IConfiguration configuration,
        Assembly? assembly = null,
        Action<IRabbitMqBusFactoryConfigurator, IBusRegistrationContext>? configureBus = null)
    {
        // Get settings from configuration
        var rabbitMqSettings = configuration.GetSection(RabbitMqSettings.Section).Get<RabbitMqSettings>() ??
                               throw new Exception("RabbitMqSettings are not configured");

        // Register settings for DI
        services.Configure<RabbitMqSettings>(configuration.GetSection(RabbitMqSettings.Section));

        services.AddMassTransit(x =>
        {
            // Register consumers from the provided assembly or the executing assembly if none provided
            if (assembly != null)
            {
                x.AddConsumers(assembly);
            }

            x.SetKebabCaseEndpointNameFormatter();

            // Exclude base types from being treated as message types
            x.UsingRabbitMq((context, cfg) =>
            {
                // Use the settings directly instead of trying to resolve from DI
                cfg.Host(new Uri(rabbitMqSettings.Host), rabbitMqSettings.VirtualHost, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                // Will retry 5 times (6 total attempts including original)
                // Each retry is 5 seconds. If retry hit limit, it will move to error queue for tracking
                
                cfg.UseMessageRetry(r => r.Interval(
                    retryCount: 5,
                    interval: TimeSpan.FromSeconds(5)
                ));

                configureBus?.Invoke(cfg, context);

                // Configure endpoints for all registered consumers
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IMessageBusService, MassTransitService>();
    }

    /// <summary>
    ///     Adds encryption service to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <exception cref="Exception"></exception>
    public static void AddEncryptionService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(EncryptionSettings.Section).Get<EncryptionSettings>() ??
            throw new Exception("EncryptionSettings are not configured");
        services.Configure<EncryptionSettings>(configuration.GetSection(EncryptionSettings.Section));
        services.AddSingleton<IEncryptionService, AesEncryptionService>();
    }
    
    public static void AddApiService(this IServiceCollection services)
    {
        services.AddHttpClient<ApiServices>();
    }
    
    public static void AddQuartzService(this IServiceCollection services)
    {
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddQuartz();
        services.AddSingleton(
            provider =>
            {
                var scheduler = provider.GetRequiredService<ISchedulerFactory>().GetScheduler().Result;
                // Must have job factory to register job
                scheduler.JobFactory = provider.GetRequiredService<IJobFactory>();
                return scheduler;
            });
        services.AddSingleton<ISchedulerService, QuartzSchedulerService>();
    }

    public static void AddFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure file storage settings
        _ = configuration.GetSection(FileStorageSettings.Section).Get<FileStorageSettings>()
            ?? throw new InvalidOperationException("FileStorageSettings is not configured properly.");
        services.Configure<FileStorageSettings>(configuration.GetSection(FileStorageSettings.Section));

        // Register file storage service
        services.AddScoped<IFileStorageService, FileStorageService>();
    }
    
}