using System.Reflection;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Infrastructure.Security.Encryption;
using EngConnect.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Tests.Common;

internal static class TestTypeCatalog
{
    private static readonly string[] ExcludedKeywords = ["course", "module", "session", "resource"];

    private static readonly Assembly ApplicationAssembly = typeof(global::EngConnect.Application.AssemblyReference).Assembly;
    private static readonly Assembly PresentationAssembly = typeof(global::EngConnect.Presentation.AssemblyReference).Assembly;

    private static readonly Assembly[] ServiceAssemblies =
    [
        typeof(AesEncryptionService).Assembly,
        typeof(MessageBusWithOutboxService).Assembly
    ];

    public static IReadOnlyList<Type> GetIncludedControllerTypes()
    {
        return PresentationAssembly
            .GetTypes()
            .Where(type => type.IsClass
                           && !type.IsAbstract
                           && type.Namespace == "EngConnect.Presentation.Controllers"
                           && type.Name.EndsWith("Controller", StringComparison.Ordinal)
                           && IsIncluded(type))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToList();
    }

    public static IReadOnlyList<MethodInfo> GetControllerActionMethods(Type controllerType)
    {
        return controllerType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(method => method.ReturnType == typeof(Task<IActionResult>))
            .OrderBy(method => method.Name, StringComparer.Ordinal)
            .ToList();
    }

    public static IReadOnlyList<Type> GetIncludedHandlerTypes()
    {
        return ApplicationAssembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && type.Namespace != null
                           && type.Namespace.Contains(".UseCases.", StringComparison.Ordinal)
                           && GetHandlerInterfaces(type).Count != 0
                           && IsIncluded(type))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToList();
    }

    public static IReadOnlyList<Type> GetIncludedValidatorTypes()
    {
        return ApplicationAssembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && type.Namespace != null
                           && type.Namespace.Contains(".UseCases.", StringComparison.Ordinal)
                           && IsValidator(type)
                           && IsIncluded(type))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToList();
    }

    public static IReadOnlyList<Type> GetHandlerInterfaces(Type handlerType)
    {
        return handlerType
            .GetInterfaces()
            .Where(@interface => @interface.IsGenericType
                                 && @interface.GetGenericTypeDefinition() is var definition
                                 && (definition == typeof(ICommandHandler<>)
                                     || definition == typeof(ICommandHandler<,>)
                                     || definition == typeof(IQueryHandler<,>)))
            .ToList();
    }

    public static Type GetHandlerRequestType(Type handlerType)
    {
        return GetHandlerInterfaces(handlerType)
            .Select(@interface => @interface.GenericTypeArguments[0])
            .First();
    }

    public static Type GetValidatorRequestType(Type validatorType)
    {
        var validatorInterface = validatorType
            .GetInterfaces()
            .First(@interface => @interface.IsGenericType
                                 && @interface.GetGenericTypeDefinition() == typeof(IValidator<>));

        return validatorInterface.GenericTypeArguments[0];
    }

    public static IReadOnlyList<Type> GetIncludedServiceTypes(params Assembly[] assemblies)
    {
        var targetAssemblies = assemblies.Length == 0 ? ServiceAssemblies : assemblies;

        return targetAssemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && !type.IsGenericTypeDefinition
                           && IsIncluded(type)
                           && (type.Name.EndsWith("Service", StringComparison.Ordinal)
                               || type.Name.EndsWith("Services", StringComparison.Ordinal)))
            .Distinct()
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .ToList();
    }

    public static bool IsIncluded(MemberInfo member)
    {
        var candidate = $"{member.DeclaringType?.FullName}::{member.Name}";
        return ExcludedKeywords.All(keyword =>
            candidate.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0);
    }

    public static bool IsIncluded(Type type)
    {
        var candidate = $"{type.FullName}|{type.Namespace}|{type.Name}";
        return ExcludedKeywords.All(keyword =>
            candidate.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) < 0);
    }

    private static bool IsValidator(Type type)
    {
        return type.GetInterfaces()
            .Any(@interface => @interface.IsGenericType
                               && @interface.GetGenericTypeDefinition() == typeof(IValidator<>));
    }
}
