namespace EngConnect.Tests.Common;

public sealed class UseCaseTestCase
{
    public required string Name { get; init; }

    public required UseCaseScenario Scenario { get; init; }

    public override string ToString()
    {
        return Name;
    }
}
