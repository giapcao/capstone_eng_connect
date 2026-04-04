namespace EngConnect.Tests.Common;

public sealed class UseCaseCaseCatalog
{
    public required IReadOnlyList<UseCaseCaseSet> ValidCases { get; init; }

    public required IReadOnlyList<UseCaseCaseSet> BoundaryCases { get; init; }

    public required IReadOnlyList<UseCaseCaseSet> InvalidCases { get; init; }

    public required IReadOnlyList<UseCaseCaseSet> ExceptionCases { get; init; }

    public IEnumerable<object[]> HandlerCases()
    {
        return GetAllCases()
            .Where(caseSet => caseSet.HandlerExpectation != UseCaseHandlerExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public IEnumerable<object[]> ValidatorCases()
    {
        return GetAllCases()
            .Where(caseSet => caseSet.ValidatorExpectation != UseCaseValidatorExpectation.Skip)
            .Select(caseSet => new object[] { caseSet });
    }

    public IEnumerable<UseCaseCaseSet> GetAllCases()
    {
        foreach (var caseSet in ValidCases)
        {
            yield return caseSet;
        }

        foreach (var caseSet in BoundaryCases)
        {
            yield return caseSet;
        }

        foreach (var caseSet in InvalidCases)
        {
            yield return caseSet;
        }

        foreach (var caseSet in ExceptionCases)
        {
            yield return caseSet;
        }
    }

    public UseCaseCaseSet GetCaseByName(string name)
    {
        return GetAllCases().Single(caseSet => string.Equals(caseSet.Name, name, StringComparison.Ordinal));
    }
}
