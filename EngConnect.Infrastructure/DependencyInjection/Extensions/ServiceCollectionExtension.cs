using Amazon.S3;
using EngConnect.Application.Abstraction;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Infrastructure.DependencyInjection.Extensions;
using EngConnect.BuildingBlock.Infrastructure.JWT;
using EngConnect.Domain.Abstraction;
using EngConnect.Infrastructure.AiService;
using EngConnect.Infrastructure.EmailService;
using EngConnect.Infrastructure.HostedService;
using EngConnect.Infrastructure.FileStorageService;
using EngConnect.Infrastructure.JWT;
using EngConnect.Infrastructure.Persistence;
using EngConnect.Infrastructure.Persistence.Data;
using EngConnect.Infrastructure.Persistence.Repositories;
using EngConnect.Infrastructure.Quartz.OutboxEvent;
using EngConnect.Infrastructure.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mscc.GenerativeAI;
using ClientSecrets = Google.Apis.Auth.OAuth2.ClientSecrets;

namespace EngConnect.Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
         services.AddPersistence(configuration);
        // services.ApplyMigration();
        services.AddUnitOfWork();
        services.AddMessageBus(configuration, AssemblyReference.Assembly, ConfigureConsumers);
        services.AddRedis(configuration);
        services.AddQuartzService();
        services.AddSchedulers(configuration);
        // services.AddReportService();
        services.AddHostedService();
        services.AddRedisCacheSettings(configuration);
        services.AddJwtSettings(configuration);
        services.AddAuthenticationServices();
        // services.AddMailKitEmailService(configuration);
        services.AddGmailApiEmailService(configuration);
        // services.AddFileStorage(configuration);
        services.AddGoogleDriveStorageService(configuration);
        services.AddAwsStorageSettings(configuration);
        services.AddMessageBusWithOutboxService();
        services.AddGitHubModelsSettings(configuration);
    }


    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("ConnectionStrings:DefaultConnection is not configured");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void AddHostedService(this IServiceCollection services)
    {
        services.AddHostedService<AppHostedService>();
    }

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
        services.AddScoped<IOutboxEventRepository, OutboxEventRepository>();
    }


    /// <summary>
    ///     Add redis cache TTL settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <exception cref="Exception"></exception>
    private static void AddRedisCacheSettings(this IServiceCollection services, IConfiguration configuration)
    {
         _= configuration.GetSection(RedisCacheSettings.Section).Get<RedisCacheSettings>() ??
            throw new Exception("RedisCacheSettings are not configured");
        services.Configure<RedisCacheSettings>(configuration.GetSection(RedisCacheSettings.Section));
    }

    private static void AddSchedulers(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(ScheduleJobSettings.Section).Get<ScheduleJobSettings>() ??
            throw new Exception("AppSettings are not configured");

        services.Configure<ScheduleJobSettings>(configuration.GetSection(ScheduleJobSettings.Section));
        services.AddScoped<IOutboxEventScheduler, OutboxEventScheduler>();
    }

    private static void AddMailKitEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(EmailSettings.Section).Get<EmailSettings>() ??
            throw new Exception("EmailSettings are not configured");
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));
        services.AddScoped<IEmailService, MailKitEmailService>();
    }

    private static void AddGmailApiEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(GmailApiSettings.Section).Get<GmailApiSettings>() ??
                       throw new Exception("GmailApiSettings are not configured");

        if (string.IsNullOrWhiteSpace(settings.ClientId) ||
            string.IsNullOrWhiteSpace(settings.ClientSecret) ||
            string.IsNullOrWhiteSpace(settings.RefreshToken) ||
            string.IsNullOrWhiteSpace(settings.SenderEmail))
        {
            throw new Exception("GmailApiSettings must include ClientId, ClientSecret, RefreshToken, and SenderEmail");
        }

        services.Configure<GmailApiSettings>(configuration.GetSection(GmailApiSettings.Section));

        services.AddSingleton<GmailService>(_ =>
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret
            };
            

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = [GmailService.Scope.GmailSend],
                DataStore = null
            });

            var tokenResponse = new TokenResponse
            {
                RefreshToken = settings.RefreshToken
            }; 

            var credential = new UserCredential(flow, settings.SenderEmail, tokenResponse);

            return new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = settings.ApplicationName
            });
        });

        services.AddScoped<IEmailService, GmailApiEmailService>();
    }


    private static void AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
    {
        _ = configuration.GetSection(JwtSettings.Section).Get<JwtSettings>() ??
            throw new Exception("JwtSettings are not configured");
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));
    }

    private static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>(); 
        services.AddScoped<IClaimsExtractor, ClaimsExtractor>();
    }
    
    public static void AddMessageBusWithOutboxService(this IServiceCollection services)
    {
        services.AddScoped<IMessageBusWithOutboxService, MessageBusWithOutboxService>();
    }
    
    private static void AddGoogleDriveStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(GoogleDriveSettings.Section).Get<GoogleDriveSettings>() ??
                       throw new Exception("GoogleDriveSettings are not configured");
    
        services.Configure<GoogleDriveSettings>(configuration.GetSection(GoogleDriveSettings.Section));

        services.AddSingleton<DriveService>(_ =>
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = settings.ClientId,
                ClientSecret = settings.ClientSecret
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = [DriveService.Scope.Drive],
                DataStore = null 
            });

            var tokenResponse = new TokenResponse
            {
                RefreshToken = settings.RefreshToken
            };
            
            var credential = new UserCredential(flow, "user", tokenResponse);
            
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = settings.ApplicationName
            });
        });

        services.AddScoped<IDriveService, GoogleDriveService>();
    }
    
    private static void AddAwsStorageSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var awsSettings = configuration.GetSection(AwsStorageSettings.Section).Get<AwsStorageSettings>() ??
                              throw new Exception("AwsStorageSettings are not configured");
            services.Configure<AwsStorageSettings>(configuration.GetSection(AwsStorageSettings.Section));
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var s3Config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsSettings.Region)
                };
                return new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, s3Config);
            });
            services.AddScoped<IAwsStorageService, AwsS3Service>();
        }
    
    private static void AddGeminiSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var geminiSettings = configuration.GetSection(GeminiApiKeySetting.Section).Get<GeminiApiKeySetting>() 
                                 ?? throw new Exception($"{GeminiApiKeySetting.Section} are not configured");
            
            services.Configure<GeminiApiKeySetting>(configuration.GetSection(GeminiApiKeySetting.Section));
            
            services.AddSingleton<IGenerativeAI>(sp => new GoogleAI(geminiSettings.ApiKey));
            services.AddScoped<IAiService, AiSummarizeService>();
        }
    
    private static void AddGitHubModelsSettings(this IServiceCollection services, IConfiguration configuration)
    {
        _= configuration.GetSection(GitHubModelsSettings.Section).Get<GitHubModelsSettings>() 
                             ?? throw new Exception($"{GitHubModelsSettings.Section} is not configured");

        services.Configure<GitHubModelsSettings>(configuration.GetSection(GitHubModelsSettings.Section));
        
        services.AddHttpClient<IAiService, GitHubModelAiService>(client => 
        {
            client.Timeout = TimeSpan.FromSeconds(60);
        });
    }
}