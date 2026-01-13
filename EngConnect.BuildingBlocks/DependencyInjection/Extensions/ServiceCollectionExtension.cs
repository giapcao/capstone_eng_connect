using System.Reflection;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Validation.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace EngConnect.BuildingBlock.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///     Adds CQRS services to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly">The assembly to scan for handlers</param>
    public static void AddCqrs(this IServiceCollection services, Assembly assembly)
    {
        // Register dispatchers
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        // Register all command handlers with validation decorators
        RegisterHandlersWithValidation(services, typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>), assembly);

        // Register all command handlers without result (using the correct decorator)
        RegisterHandlersWithValidation(services, typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>), assembly);
        RegisterHandlersWithValidation(services, typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>), assembly);

        // Register all query handlers with validation decorators
        RegisterHandlersWithValidation(services, typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>), assembly);
    }

    private static void RegisterHandlersWithValidation(
        IServiceCollection services,
        Type handlerInterfaceType,
        Type decoratorType,
        Assembly assembly)
    {
        // Find all concrete handler implementations
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                        t.GetInterfaces().Any(i => i.IsGenericType &&
                                                   i.GetGenericTypeDefinition() == handlerInterfaceType));

        foreach (var handlerType in handlerTypes)
        {
            // Get the handler interface that this concrete type implements
            var handlerInterface = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);

            // Get the generic type arguments from the interface (TCommand/TQuery and TResult)
            var genericArgs = handlerInterface.GetGenericArguments();

            // Create a specific decorator type with the correct generic arguments
            var decoratorGenericType = decoratorType.MakeGenericType(genericArgs);

            // Register the concrete handler implementation
            services.AddScoped(handlerType);

            // Register the decorator with the interface
            services.AddScoped(handlerInterface, serviceProvider =>
            {
                // Resolve the concrete handler
                var handler = serviceProvider.GetRequiredService(handlerType);

                // Create the decorator, passing the handler and service provider
                return Activator.CreateInstance(
                    decoratorGenericType,
                    handler,
                    serviceProvider)!;
            });
        }
    }
}