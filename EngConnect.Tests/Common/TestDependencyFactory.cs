using System.Net;
using System.Reflection;
using Amazon.S3;
using Google.Apis.Drive.v3;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;

namespace EngConnect.Tests.Common;

internal static class TestDependencyFactory
{
    public static object CreateInstance(Type type, IReadOnlyDictionary<Type, object?>? overrides = null)
    {
        var constructor = type
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            .OrderByDescending(ctor => ctor.GetParameters().Length)
            .FirstOrDefault()
            ?? throw new InvalidOperationException($"Type {type.FullName} does not have a public constructor.");

        var arguments = constructor
            .GetParameters()
            .Select(parameter => ResolveDependency(parameter.ParameterType, overrides))
            .ToArray();

        return constructor.Invoke(arguments);
    }

    public static object? ResolveDependency(Type type, IReadOnlyDictionary<Type, object?>? overrides = null)
    {
        if (overrides != null && overrides.TryGetValue(type, out var overridden))
        {
            return overridden;
        }

        if (type == typeof(IServiceProvider))
        {
            return new ServiceCollection().BuildServiceProvider();
        }

        if (type == typeof(HttpClient))
        {
            return TestObjectFactory.CreateHttpClient();
        }

        if (type == typeof(DriveService))
        {
            return new DriveService(new BaseClientService.Initializer
            {
                ApplicationName = "EngConnect.Tests"
            });
        }

        if (type == typeof(GmailService))
        {
            return new GmailService(new BaseClientService.Initializer
            {
                ApplicationName = "EngConnect.Tests"
            });
        }

        if (type == typeof(IAmazonS3))
        {
            return CreateMockObject(type, MockBehavior.Loose);
        }

        if (type == typeof(IUnitOfWork))
        {
            return new InMemoryUnitOfWork();
        }

        if (type == typeof(IConnectionMultiplexer))
        {
            return CreateRedisMultiplexer();
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ILogger<>))
        {
            var loggerType = typeof(Logger<>).MakeGenericType(type.GenericTypeArguments[0]);
            return Activator.CreateInstance(loggerType, NullLoggerFactory.Instance)!;
        }

        if (type == typeof(ILogger))
        {
            return NullLogger.Instance;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IOptions<>))
        {
            return TestObjectFactory.CreateOptions(type.GenericTypeArguments[0]);
        }

        if (type.IsInterface || type.IsAbstract)
        {
            return CreateMockObject(type, MockBehavior.Loose);
        }

        return TestObjectFactory.CreateValue(type);
    }

    public static object CreateStrictDependency(Type type)
    {
        if (type == typeof(IUnitOfWork))
        {
            return new ThrowingUnitOfWork();
        }

        if (type == typeof(HttpClient))
        {
            return TestObjectFactory.CreateHttpClient((_, _) =>
                throw new HttpRequestException("Forced test failure for handler invalid-path coverage."));
        }

        if (type.IsInterface || type.IsAbstract)
        {
            return CreateMockObject(type, MockBehavior.Strict);
        }

        return TestObjectFactory.CreateValue(type)
               ?? throw new InvalidOperationException($"Cannot create strict dependency for {type.FullName}.");
    }

    private static object CreateMockObject(Type type, MockBehavior behavior)
    {
        var mockType = typeof(Mock<>).MakeGenericType(type);
        var mock = (Mock)Activator.CreateInstance(mockType, behavior)!;
        mock.DefaultValue = DefaultValue.Mock;
        return mock.Object;
    }

    private static IConnectionMultiplexer CreateRedisMultiplexer()
    {
        var database = new Mock<IDatabase>(MockBehavior.Loose);
        var server = new Mock<IServer>(MockBehavior.Loose);
        var multiplexer = new Mock<IConnectionMultiplexer>(MockBehavior.Loose)
        {
            DefaultValue = DefaultValue.Mock
        };

        var endpoint = new DnsEndPoint("localhost", 6379);

        multiplexer.Setup(value => value.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(database.Object);
        multiplexer.Setup(value => value.GetEndPoints(It.IsAny<bool>()))
            .Returns([endpoint]);
        multiplexer.Setup(value => value.GetServer(It.IsAny<EndPoint>(), It.IsAny<object>()))
            .Returns(server.Object);

        database.SetupGet(value => value.Multiplexer)
            .Returns(multiplexer.Object);

        return multiplexer.Object;
    }
}
