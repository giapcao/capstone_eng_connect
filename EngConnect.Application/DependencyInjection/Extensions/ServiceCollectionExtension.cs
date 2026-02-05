using System.Reflection.Metadata;
using EngConnect.Application.Mapping;
using EngConnect.Application.UseCases.Authentication.LoginByUser;
using EngConnect.Application.UseCases.Authentication.RefreshToken;
using EngConnect.Application.UseCases.Authentication.RegisterUser;
using EngConnect.Application.UseCases.Authentication.VerifyEmail;
using EngConnect.Application.UseCases.Users.ChangePassword;
using EngConnect.Application.UseCases.Users.CreateUser;
using EngConnect.Application.UseCases.Users.ForgotPassword;
using EngConnect.Application.UseCases.Users.ResetPassword;
using EngConnect.Application.UseCases.Users.UpdateUser;
using EngConnect.BuildingBlock.DependencyInjection.Extensions;
using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddValidators();
        services.AddCqrs(AssemblyReference.Assembly);
        services.AddMappings();
    }

    private static void AddMappings(this IServiceCollection services)
    {
        services.AddScoped<IMapper, Mapper>();
        MappingConfig.RegisterMappings();
    }

    private static void AddValidators(this IServiceCollection services)
    {
        // Explicitly register specific validators if necessary
        // services.AddScoped<IValidator<COMMAND_CLASS>, COMMAND_VALIDATOR_CLASS>();

        // Authentication Section
        services.AddScoped<IValidator<LoginByUserCommand>, LoginByUserCommandValidator>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        services.AddScoped<IValidator<VerifyEmailCommand>, VerifyEmailCommandValidator>();
        services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();
        
        // User Section
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<ForgotPasswordCommand>, ForgotPasswordCommandValidator>();
        services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidator>();
        services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
    }
}