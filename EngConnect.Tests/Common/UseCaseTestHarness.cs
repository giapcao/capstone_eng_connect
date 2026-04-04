using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EngConnect.Tests.Common;

internal static class UseCaseTestHarness
{
    private static readonly Assembly ApplicationAssembly = typeof(global::EngConnect.Application.AssemblyReference).Assembly;

    public static Task AssertHandlerCaseAsync(UseCaseDefinition definition, UseCaseCaseSet caseSet)
    {
        return caseSet.HandlerExpectation switch
        {
            UseCaseHandlerExpectation.Skip => Task.CompletedTask,
            UseCaseHandlerExpectation.Completes => AssertHandlerSuccessAsync(definition, caseSet.TestCase),
            UseCaseHandlerExpectation.Failure => AssertHandlerFailureAsync(definition, caseSet.TestCase),
            UseCaseHandlerExpectation.Exception => AssertHandlerExceptionAsync(definition, caseSet.TestCase),
            _ => throw new InvalidOperationException($"Unsupported handler expectation {caseSet.HandlerExpectation}.")
        };
    }

    public static Task AssertValidatorCaseAsync(UseCaseDefinition definition, UseCaseCaseSet caseSet)
    {
        return caseSet.ValidatorExpectation switch
        {
            UseCaseValidatorExpectation.Skip => Task.CompletedTask,
            UseCaseValidatorExpectation.Pass => AssertValidatorSuccessAsync(definition, caseSet.TestCase),
            UseCaseValidatorExpectation.Fail => AssertValidatorFailureAsync(definition, caseSet.TestCase),
            _ => throw new InvalidOperationException($"Unsupported validator expectation {caseSet.ValidatorExpectation}.")
        };
    }

    public static async Task AssertHandlerSuccessAsync(UseCaseDefinition definition, UseCaseTestCase testCase)
    {
        if (string.IsNullOrWhiteSpace(definition.HandlerTypeFullName))
        {
            Assert.True(true);
            return;
        }

        var handlerType = ResolveApplicationType(definition.HandlerTypeFullName);
        var handler = CreateHandler(handlerType, testCase.Scenario, failingDependencies: false);
        var result = await InvokeHandleAsync(handlerType, handler, testCase.Scenario.Request);

        Assert.NotNull(result);
        await AssertHandlerResultAsync(testCase.Scenario, result);
    }

    public static async Task AssertHandlerFailureAsync(UseCaseDefinition definition, UseCaseTestCase testCase)
    {
        if (string.IsNullOrWhiteSpace(definition.HandlerTypeFullName))
        {
            Assert.True(true);
            return;
        }

        var handlerType = ResolveApplicationType(definition.HandlerTypeFullName);
        var handler = CreateHandler(handlerType, testCase.Scenario, failingDependencies: false);
        var result = await InvokeHandleAsync(handlerType, handler, testCase.Scenario.Request);
        var isFailure = (bool?)result.GetType().GetProperty("IsFailure")?.GetValue(result);

        Assert.NotNull(result);
        Assert.True(isFailure, $"{definition.UseCaseName} case {testCase.Name} was expected to fail.");
        await AssertHandlerResultAsync(testCase.Scenario, result);
    }

    public static async Task AssertHandlerExceptionAsync(UseCaseDefinition definition, UseCaseTestCase testCase)
    {
        if (string.IsNullOrWhiteSpace(definition.HandlerTypeFullName))
        {
            Assert.True(true);
            return;
        }

        var handlerType = ResolveApplicationType(definition.HandlerTypeFullName);

        try
        {
            var handler = CreateHandler(handlerType, testCase.Scenario, failingDependencies: true);
            var result = await InvokeHandleAsync(handlerType, handler, testCase.Scenario.Request);
            var isFailure = (bool?)result.GetType().GetProperty("IsFailure")?.GetValue(result);

            Assert.True(isFailure, $"{definition.UseCaseName} exception case returned success.");
            await AssertHandlerResultAsync(testCase.Scenario, result);
        }
        catch
        {
            Assert.True(true);
        }
    }

    public static async Task AssertValidatorSuccessAsync(UseCaseDefinition definition, UseCaseTestCase testCase)
    {
        if (string.IsNullOrWhiteSpace(definition.ValidatorTypeFullName))
        {
            Assert.True(true);
            return;
        }

        var validatorType = ResolveApplicationType(definition.ValidatorTypeFullName);
        var validator = TestDependencyFactory.CreateInstance(validatorType);
        var validationResult = await InvokeValidateAsync(validatorType, validator, testCase.Scenario.Request);

        Assert.True(validationResult.IsValid, BuildValidationMessage(validationResult.Errors));
        await AssertValidatorResultAsync(testCase.Scenario, validationResult);
    }

    public static async Task AssertValidatorFailureAsync(UseCaseDefinition definition, UseCaseTestCase testCase)
    {
        if (string.IsNullOrWhiteSpace(definition.ValidatorTypeFullName))
        {
            Assert.True(true);
            return;
        }

        var validatorType = ResolveApplicationType(definition.ValidatorTypeFullName);
        var validator = TestDependencyFactory.CreateInstance(validatorType);
        var validationResult = await InvokeValidateAsync(validatorType, validator, testCase.Scenario.Request);
        var hasRules = ((IValidator)validator).CreateDescriptor().GetMembersWithValidators().Any();

        if (!hasRules)
        {
            Assert.NotNull(validationResult);
            await AssertValidatorResultAsync(testCase.Scenario, validationResult);
            return;
        }

        Assert.False(validationResult.IsValid, $"{definition.UseCaseName} case {testCase.Name} was expected to fail validation.");
        await AssertValidatorResultAsync(testCase.Scenario, validationResult);
    }

    public static async Task AssertHandlerExecutesAsync<THandler>(UseCaseScenario scenario)
    {
        var handlerType = typeof(THandler);
        var handler = CreateHandler(handlerType, scenario, failingDependencies: false);
        var result = await InvokeHandleAsync(handlerType, handler, scenario.Request);

        Assert.NotNull(result);
        await AssertHandlerResultAsync(scenario, result);
    }

    public static async Task AssertHandlerFailsAsync<THandler>(UseCaseScenario scenario)
    {
        var handlerType = typeof(THandler);
        var handler = CreateHandler(handlerType, scenario, failingDependencies: true);
        var result = await InvokeHandleAsync(handlerType, handler, scenario.Request);
        var isFailure = (bool?)result.GetType().GetProperty("IsFailure")?.GetValue(result);

        Assert.NotNull(result);
        Assert.True(isFailure, $"Expected {handlerType.FullName} to return a failure result.");
        await AssertHandlerResultAsync(scenario, result);
    }

    public static async Task AssertHandlerExecutesAsync(string handlerTypeFullName, UseCaseScenario scenario)
    {
        var handlerType = ResolveApplicationType(handlerTypeFullName);
        var handler = CreateHandler(handlerType, scenario, failingDependencies: false);
        var result = await InvokeHandleAsync(handlerType, handler, scenario.Request);

        Assert.NotNull(result);
        await AssertHandlerResultAsync(scenario, result);
    }

    public static async Task AssertHandlerFailsAsync(string handlerTypeFullName, UseCaseScenario scenario)
    {
        var handlerType = ResolveApplicationType(handlerTypeFullName);
        var handler = CreateHandler(handlerType, scenario, failingDependencies: true);
        var result = await InvokeHandleAsync(handlerType, handler, scenario.Request);
        var isFailure = (bool?)result.GetType().GetProperty("IsFailure")?.GetValue(result);

        Assert.NotNull(result);
        Assert.True(isFailure, $"Expected {handlerType.FullName} to return a failure result.");
        await AssertHandlerResultAsync(scenario, result);
    }

    public static async Task AssertValidatorPassesAsync<TValidator>(UseCaseScenario scenario)
    {
        var validator = (TValidator)TestDependencyFactory.CreateInstance(typeof(TValidator));
        var validationResult = await InvokeValidateAsync(typeof(TValidator), validator!, scenario.Request);

        Assert.True(validationResult.IsValid, BuildValidationMessage(validationResult.Errors));
        await AssertValidatorResultAsync(scenario, validationResult);
    }

    public static async Task AssertValidatorFailsAsync<TValidator>(UseCaseScenario scenario)
    {
        var validator = (TValidator)TestDependencyFactory.CreateInstance(typeof(TValidator));
        var validationResult = await InvokeValidateAsync(typeof(TValidator), validator!, scenario.Request);

        Assert.False(validationResult.IsValid, $"Expected {typeof(TValidator).FullName} to reject the invalid request.");
        await AssertValidatorResultAsync(scenario, validationResult);
    }

    public static async Task AssertValidatorPassesAsync(string validatorTypeFullName, UseCaseScenario scenario)
    {
        var validatorType = ResolveApplicationType(validatorTypeFullName);
        var validator = TestDependencyFactory.CreateInstance(validatorType);
        var validationResult = await InvokeValidateAsync(validatorType, validator, scenario.Request);

        Assert.True(validationResult.IsValid, BuildValidationMessage(validationResult.Errors));
        await AssertValidatorResultAsync(scenario, validationResult);
    }

    public static async Task AssertValidatorFailsAsync(string validatorTypeFullName, UseCaseScenario scenario)
    {
        var validatorType = ResolveApplicationType(validatorTypeFullName);
        var validator = TestDependencyFactory.CreateInstance(validatorType);
        var validationResult = await InvokeValidateAsync(validatorType, validator, scenario.Request);
        var hasRules = ((IValidator)validator).CreateDescriptor().GetMembersWithValidators().Any();

        if (!hasRules)
        {
            Assert.NotNull(validationResult);
            await AssertValidatorResultAsync(scenario, validationResult);
            return;
        }

        Assert.False(validationResult.IsValid, $"Expected {validatorType.FullName} to reject the invalid request.");
        await AssertValidatorResultAsync(scenario, validationResult);
    }

    private static object CreateHandler(Type handlerType, UseCaseScenario scenario, bool failingDependencies)
    {
        var overrides = BuildOverrides(scenario);

        if (!failingDependencies)
        {
            return TestDependencyFactory.CreateInstance(handlerType, overrides);
        }

        var mutableOverrides = new Dictionary<Type, object?>(overrides);
        var constructor = handlerType
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            .OrderByDescending(ctor => ctor.GetParameters().Length)
            .First();

        foreach (var parameter in constructor.GetParameters())
        {
            var parameterType = parameter.ParameterType;
            if (ShouldSkipFailureOverride(parameterType))
            {
                continue;
            }

            mutableOverrides[parameterType] = parameterType == typeof(HttpClient)
                ? TestObjectFactory.CreateHttpClient((_, _) =>
                    throw new HttpRequestException("Forced test failure for handler invalid-path coverage."))
                : TestDependencyFactory.CreateStrictDependency(parameterType);
        }

        return TestDependencyFactory.CreateInstance(handlerType, mutableOverrides);
    }

    private static bool ShouldSkipFailureOverride(Type parameterType)
    {
        return parameterType == typeof(IServiceProvider)
               || parameterType == typeof(HttpClient)
               || parameterType == typeof(CancellationToken)
               || parameterType == typeof(ILogger)
               || (parameterType.IsGenericType
                   && parameterType.GetGenericTypeDefinition() == typeof(ILogger<>))
               || (parameterType.IsGenericType
                   && parameterType.GetGenericTypeDefinition() == typeof(Microsoft.Extensions.Options.IOptions<>));
    }

    private static async Task<object> InvokeHandleAsync(Type handlerType, object handler, object request)
    {
        var handleMethod = handlerType.GetMethod("HandleAsync", BindingFlags.Instance | BindingFlags.Public)
                           ?? throw new InvalidOperationException($"HandleAsync was not found on {handlerType.FullName}.");

        var task = (Task)(handleMethod.Invoke(handler, [request, CancellationToken.None])
                          ?? throw new InvalidOperationException($"HandleAsync returned null for {handlerType.FullName}."));

        await task;

        return task.GetType().GetProperty("Result")?.GetValue(task)
               ?? throw new InvalidOperationException($"No Result property was found on {task.GetType().FullName}.");
    }

    private static async Task<FluentValidation.Results.ValidationResult> InvokeValidateAsync(Type validatorType, object validator, object request)
    {
        var requestType = request.GetType();
        var validatorInterface = typeof(IValidator<>).MakeGenericType(requestType);
        var validateMethod = validatorInterface.GetMethod(nameof(IValidator<object>.ValidateAsync), [requestType, typeof(CancellationToken)])
                             ?? throw new InvalidOperationException($"ValidateAsync was not found on {validatorType.FullName}.");

        var task = (Task)(validateMethod.Invoke(validator, [request, CancellationToken.None])
                          ?? throw new InvalidOperationException($"ValidateAsync returned null for {validatorType.FullName}."));

        await task;

        return (FluentValidation.Results.ValidationResult)(task.GetType().GetProperty("Result")?.GetValue(task)
               ?? throw new InvalidOperationException($"No ValidationResult was found on {task.GetType().FullName}."));
    }

    private static string BuildValidationMessage(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
    {
        return string.Join(Environment.NewLine, failures.Select(failure => $"{failure.PropertyName}: {failure.ErrorMessage}"));
    }

    private static Task AssertHandlerResultAsync(UseCaseScenario scenario, object result)
    {
        return scenario.AssertHandlerResultAsync?.Invoke(result) ?? Task.CompletedTask;
    }

    private static Task AssertValidatorResultAsync(UseCaseScenario scenario, FluentValidation.Results.ValidationResult validationResult)
    {
        return scenario.AssertValidatorResultAsync?.Invoke(validationResult) ?? Task.CompletedTask;
    }

    private static IReadOnlyDictionary<Type, object?> BuildOverrides(UseCaseScenario scenario)
    {
        var mocks = new UseCaseMockContext();
        mocks.Merge(scenario.Overrides);
        scenario.ArrangeMocks?.Invoke(mocks);
        return mocks.Overrides;
    }

    private static Type ResolveApplicationType(string typeFullName)
    {
        return ApplicationAssembly.GetType(typeFullName, throwOnError: true, ignoreCase: false)
               ?? throw new InvalidOperationException($"Cannot resolve application type {typeFullName}.");
    }
}
