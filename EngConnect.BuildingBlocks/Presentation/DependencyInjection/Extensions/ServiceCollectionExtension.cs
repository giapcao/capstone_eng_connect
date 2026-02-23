using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Presentation.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace EngConnect.BuildingBlock.Presentation.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///     Configures Swagger/OpenAPI documentation
    /// </summary>
    public static void ConfigureSwagger(this WebApplicationBuilder builder, string serviceName, string version, Assembly[] xmlAssemblies)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc(version, new OpenApiInfo
            {
                Title = $"{serviceName}",
                Version = version,
                Description = $"{serviceName}"
            });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // opt.AddScalarFilters();
            // Optional: Add XML comments if you have them
            if (ValidationUtil.IsNotNullOrEmpty(xmlAssemblies))
            {
                foreach (var xmlAssembly in xmlAssemblies)
                {
                    var xmlFile = $"{xmlAssembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    opt.EnableAnnotations();
                    if (File.Exists(xmlPath))
                    {
                        opt.IncludeXmlComments(xmlPath, true);
                    }
                }
            }

            // Add schema filter to handle circular references
            // opt.CustomSchemaIds(type => type.FullName);

            // Configure nullable reference types
            // opt.SupportNonNullableReferenceTypes();
        });
    }

    /// <summary>
    ///     Configures Scalar API Reference
    /// </summary>
    /// <param name="app"></param>
    /// <param name="serviceName"></param>
    public static void UseScalarApiReference(this WebApplication app, string serviceName)
    {
        app.UseSwagger(options => { options.RouteTemplate = "/openapi/{documentName}.json"; });
        app.MapScalarApiReference(options =>
        {
            options.WithTitle($"{serviceName}")
                .WithDefaultHttpClient(ScalarTarget.Http, ScalarClient.Http11)
                .WithOperationSorter(OperationSorter.Alpha)
                .WithTheme(ScalarTheme.Kepler)
                .WithLayout(ScalarLayout.Modern)
                .WithSidebar(true)
                .WithClientButton(true)
                .WithDownloadButton(true)
                .WithDarkModeToggle(true);
        });
    }

    /// <summary>
    ///     Configures Swagger API Reference
    /// </summary>
    /// <param name="app"></param>
    /// <param name="serviceName"></param>
    /// <param name="version"></param>
    /// <param name="cacheLifetimeMinutes"></param>
    // ReSharper disable once InconsistentNaming
    public static void UseSwaggerApiReference(this WebApplication app, string serviceName, string version,
        int cacheLifetimeMinutes = 1)
    {
        app.UseSwagger(options => { options.RouteTemplate = "/openapi/{documentName}.json"; });
        // Add Swagger UI
        app.UseSwaggerUI(c =>
        {
            // API Gateway's own Swagger
            c.SwaggerEndpoint($"/openapi/{version}.json", $"{serviceName}");

            // [25-05-2025] Should set small cache lifetime to avoid browser cache on development
            c.CacheLifetime = TimeSpan.FromMinutes(cacheLifetimeMinutes);
        });
    }

    public static void UseRedoc(this WebApplication app, string documentTitle, string specUrl, string routePrefix)
    {
        app.UseReDoc(c =>
        {
            c.DocumentTitle = documentTitle;
            c.SpecUrl = specUrl;
            c.RoutePrefix = routePrefix; // Set as default documentation
            c.HeadContent = """

                                                <style>
                                                    /* Target operation summaries and headers in ReDoc */
                                                    .operation-summary,
                                                    [data-section-id] h1, 
                                                    [data-section-id] h2,
                                                    .redoc-markdown h1,
                                                    .redoc-markdown h2 {
                                                        font-weight: bold !important;
                                                        color: rgb(50, 50, 159) !important;
                                                                     }                   
                                                </style>
                                            
                            """;
        });
    }

    /// <summary>
    ///     Configures JWT authentication
    /// </summary>
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection(JwtSettings.Section).Get<JwtSettings>() ??
            throw new Exception("JwtSettings are not configured");

        // RSA (Rivest-Shamir-Adleman) is an asymmetric encryption algorithm
        // In JWT context, we use RSA for digital signatures:
        // - The private key (kept secret) is used to sign tokens
        // - The public key (shared openly) is used to verify the signature
        // This is more secure than symmetric algorithms (HMAC) because the verification
        // public key doesn't need to be kept secret
        // var rsa = RSA.Create();


        // Get the RSA public key directly from the service

        var rsaPublicKey = jwtSettings.PublicKeyBytes.ReadRsaKeyBase64();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new RsaSecurityKey(rsaPublicKey),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = "role"
                    };
                }
            );
        // [20-05-2025] This is needed to disable mapping of claims from JWT to .NET claims.
        // Ex: the "sub" claim will be mapped to ClaimTypes.NameIdentifier
        // We don't want that, we want to keep the original claim name
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    }
    
    public static void AddGoogleAuthentication(this WebApplicationBuilder builder)
    {
        var googleOAuthSettings = builder.Configuration.GetSection(GoogleSettings.Section).Get<GoogleSettings>() ??
                                  throw new Exception("GoogleSettings are not configured");
        
        var redirectUrlSettings = builder.Configuration.GetSection(RedirectUrlSettings.Section).Get<RedirectUrlSettings>() ??
                                  throw new Exception("RedirectUrlSettings are not configured");
        
        //Configure settings for DI
        builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection(GoogleSettings.Section));
        builder.Services.Configure<RedirectUrlSettings>(builder.Configuration.GetSection(RedirectUrlSettings.Section));
        
        //Get authentication builder
        var authenticationBuilder = builder.Services.AddAuthentication();
        
        //Update default sign-in scheme to use cookies for external providers
        builder.Services.Configure<AuthenticationOptions>(options =>
        {
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        });
        
        //Add Cookie
        authenticationBuilder.AddCookie(options =>
        {
            //Default secure configuration
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        });
        
        //Add Google Authentication
        authenticationBuilder.AddGoogle(options =>
        {
            options.ClientId = googleOAuthSettings.ClientId;
            options.ClientSecret = googleOAuthSettings.ClientSecret;

            options.Events = new OAuthEvents
            {
                OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                },

                OnRemoteFailure = context =>
                {
                    var error = context.Failure?.Message ?? "Unknown error";

                    //Redirect to failure URL
                    context.Response.Redirect(redirectUrlSettings.GoogleLoginFailedUrl);
                    context.HandleResponse();

                    return Task.CompletedTask;
                }
            };
        });

    }

    public static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            // Register all features as policies
            foreach (var permission in PermissionHelpers.GetAllPermissions())
            {
                options.AddPolicy(permission,
                    policy => { policy.Requirements.Add(new PermissionRequirement(permission)); });
            }
        });

        // Register the FeatureAuthorizationHandler
        builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}