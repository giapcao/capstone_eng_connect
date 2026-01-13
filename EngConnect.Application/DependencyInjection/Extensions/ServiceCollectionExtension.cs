using System.Reflection.Metadata;
using EngConnect.Application.Mapping;
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
        // services.AddScoped<IValidator<RegisterAccountByCustomerCommand>, RegisterAccountByCustomerCommandValidator>();
        // services.AddScoped<IValidator<LoginByCustomerCommand>, LoginByCustomerCommandValidator>();
        
    }
}