namespace EngConnect.Tests.Common;

public sealed class UseCaseCaseSet
{
    public required string Name { get; init; }

    public required UseCaseCaseKind Kind { get; init; }

    public required UseCaseTestCase TestCase { get; init; }

    public required UseCaseHandlerExpectation HandlerExpectation { get; init; }

    public required UseCaseValidatorExpectation ValidatorExpectation { get; init; }

    public override string ToString()
    {
        return $"{Kind}:{Name}";
    }
}
