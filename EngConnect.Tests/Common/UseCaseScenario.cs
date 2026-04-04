namespace EngConnect.Tests.Common;

public sealed class UseCaseScenario
{
    public required object Request { get; init; }

    public IReadOnlyDictionary<Type, object?> Overrides { get; init; } = new Dictionary<Type, object?>();

    public Action<UseCaseMockContext>? ArrangeMocks { get; init; }

    public Func<object, Task>? AssertHandlerResultAsync { get; init; }

    public Func<FluentValidation.Results.ValidationResult, Task>? AssertValidatorResultAsync { get; init; }
}
