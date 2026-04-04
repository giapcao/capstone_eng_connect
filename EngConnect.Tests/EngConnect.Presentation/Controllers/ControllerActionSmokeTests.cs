using System.Reflection;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.Tests.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace EngConnect.Tests.EngConnect.Presentation.Controllers;

public class ControllerActionSmokeTests
{
    public static IEnumerable<object[]> ControllerCases()
    {
        return TestTypeCatalog.GetIncludedControllerTypes().Select(controllerType => new object[] { controllerType });
    }

    public static IEnumerable<object[]> ControllerActionCases()
    {
        foreach (var controllerType in TestTypeCatalog.GetIncludedControllerTypes())
        {
            foreach (var actionMethod in TestTypeCatalog.GetControllerActionMethods(controllerType))
            {
                yield return [controllerType, actionMethod];
            }
        }
    }

    [Theory]
    [MemberData(nameof(ControllerCases))]
    public void Controllers_are_api_controllers_with_routes(Type controllerType)
    {
        Assert.True(controllerType.IsDefined(typeof(ApiControllerAttribute), inherit: true),
            $"{controllerType.Name} is missing ApiControllerAttribute.");

        Assert.True(controllerType.IsDefined(typeof(RouteAttribute), inherit: true),
            $"{controllerType.Name} is missing RouteAttribute.");
    }

    [Theory]
    [MemberData(nameof(ControllerActionCases))]
    public void Controller_actions_have_http_method_attributes(Type controllerType, MethodInfo actionMethod)
    {
        var hasHttpMethodAttribute = actionMethod
            .GetCustomAttributes(inherit: true)
            .OfType<HttpMethodAttribute>()
            .Any();

        Assert.True(hasHttpMethodAttribute, $"{controllerType.Name}.{actionMethod.Name} is missing an HTTP method attribute.");
    }

    [Theory]
    [MemberData(nameof(ControllerActionCases))]
    public async Task Controller_actions_can_be_invoked_with_generated_inputs(Type controllerType, MethodInfo actionMethod)
    {
        var commandDispatcher = new RecordingCommandDispatcher();
        var queryDispatcher = new RecordingQueryDispatcher();

        var controller = (ControllerBase)TestDependencyFactory.CreateInstance(controllerType, new Dictionary<Type, object?>
        {
            [typeof(ICommandDispatcher)] = commandDispatcher,
            [typeof(IQueryDispatcher)] = queryDispatcher,
            [typeof(IOptions<RedirectUrlSettings>)] = TestObjectFactory.CreateOptions(typeof(RedirectUrlSettings))
        });

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = CreateHttpContext()
        };

        var arguments = actionMethod
            .GetParameters()
            .Select(parameter => TestObjectFactory.CreateValue(parameter.ParameterType) ?? TestDependencyFactory.ResolveDependency(parameter.ParameterType))
            .ToArray();

        var response = await InvokeActionAsync(controller, actionMethod, arguments);

        Assert.NotNull(response);
        Assert.False(response is UnauthorizedResult, $"{controllerType.Name}.{actionMethod.Name} unexpectedly returned Unauthorized.");
        Assert.True(commandDispatcher.DispatchCount + queryDispatcher.DispatchCount > 0,
            $"{controllerType.Name}.{actionMethod.Name} did not dispatch any command or query.");
    }

    private static async Task<IActionResult> InvokeActionAsync(ControllerBase controller, MethodInfo actionMethod, object?[] arguments)
    {
        try
        {
            var task = (Task<IActionResult>)(actionMethod.Invoke(controller, arguments)
                                             ?? throw new InvalidOperationException($"Invocation of {actionMethod.Name} returned null."));

            return await task;
        }
        catch (TargetInvocationException exception) when (exception.InnerException != null)
        {
            throw exception.InnerException;
        }
    }

    private static HttpContext CreateHttpContext()
    {
        var principal = TestObjectFactory.CreatePrincipal();
        var services = new ServiceCollection();
        services.AddSingleton<IAuthenticationService>(new FakeAuthenticationService(principal));

        var context = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider(),
            User = principal
        };

        context.Request.Headers.Authorization = "Bearer test-access-token";
        return context;
    }
}
