using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Infrastructure.DependencyInjection.Extensions;
using EngConnect.BuildingBlock.Infrastructure.JWT;
using EngConnect.Domain.Abstraction;
using EngConnect.Infrastructure.EmailService;
using EngConnect.Infrastructure.JWT;
using EngConnect.Infrastructure.Persistence;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddPersistence(configuration);
        // services.ApplyMigration();
        services.AddUnitOfWork();
        services.AddMessageBus(configuration, AssemblyReference.Assembly, ConfigureConsumers);
        services.AddRedis(configuration);
        // services.AddQuartzService();
        // services.AddSchedulers(configuration);
        // services.AddReportService();
        // services.AddHostedService();
        services.AddRedisCacheSettings(configuration);
        services.AddJwtSettings(configuration);
        services.AddAuthenticationServices();
        services.AddMailKitEmailService(configuration);
        services.AddFileStorage(configuration);
    }

    // private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    // {
    //     // Configure Database
    //     var connectionString = configuration.GetConnectionString("DefaultConnection") ??
    //         throw new Exception("ConnectionStrings is not configured properly");
    //     var connectionBuilder = new MySqlConnectionStringBuilder(connectionString);
    //     services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString,
    //         ServerVersion.AutoDetect(connectionBuilder.ConnectionString),
    //         e => { e.SchemaBehavior(MySqlSchemaBehavior.Ignore); }));
    // }

    // private static void AddHostedService(this IServiceCollection services)
    // {
    //     services.AddHostedService<AppHostedService>();
    // }

    private static void ConfigureConsumers(IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
    {
    }

    // private static void ApplyMigration(this IServiceCollection services)
    // {
    //     using var scope = services.BuildServiceProvider().CreateScope();
    //     var serviceProvider = scope.ServiceProvider;
    //     var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
    //     context.Database.Migrate();
    // }

    private static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // services.AddScoped<IOutboxEventRepository, OutboxEventRepository>();
    }


    /// <summary>
    ///     Add redis cache TTL settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <exception cref="Exception"></exception>
    private static void AddRedisCacheSettings(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(RedisCacheSettings.Section).Get<RedisCacheSettings>() ??
            throw new Exception("RedisCacheSettings are not configured");
        services.Configure<RedisCacheSettings>(configuration.GetSection(RedisCacheSettings.Section));
    }

    private static void AddSchedulers(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(ScheduleJobSettings.Section).Get<ScheduleJobSettings>() ??
            throw new Exception("AppSettings are not configured");

        services.Configure<ScheduleJobSettings>(configuration.GetSection(ScheduleJobSettings.Section));
        // services.AddScoped<IOutboxEventScheduler, OutboxEventScheduler>();
    }

    private static void AddMailKitEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(EmailSettings.Section).Get<EmailSettings>() ??
            throw new Exception("EmailSettings are not configured");
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));
        services.AddScoped<IEmailService, MailKitEmailService>();
    }

    private static void AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(JwtSettings.Section).Get<JwtSettings>() ??
            throw new Exception("JwtSettings are not configured");
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
    }

    // private static void AddSchedulers(this IServiceCollection services)
    // {
    // }

    private static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>(); 
        services.AddScoped<IClaimsExtractor, ClaimsExtractor>();

    }
}