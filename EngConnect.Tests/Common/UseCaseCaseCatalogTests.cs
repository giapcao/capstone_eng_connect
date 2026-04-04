using Xunit;

namespace EngConnect.Tests.Common;

public class UseCaseCaseCatalogTests
{
    [Fact]
    public void GetCaseByName_returns_matching_case_from_all_groups()
    {
        var validCase = CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass);
        var invalidCase = CreateCase("invalid-request", UseCaseCaseKind.Invalid, UseCaseHandlerExpectation.Failure, UseCaseValidatorExpectation.Fail);

        var catalog = new UseCaseCaseCatalog
        {
            ValidCases = [validCase],
            BoundaryCases = [],
            InvalidCases = [invalidCase],
            ExceptionCases = []
        };

        Assert.Same(invalidCase, catalog.GetCaseByName("invalid-request"));
    }

    [Fact]
    public void HandlerCases_and_ValidatorCases_skip_expected_entries()
    {
        var skipCase = CreateCase("skip", UseCaseCaseKind.Boundary, UseCaseHandlerExpectation.Skip, UseCaseValidatorExpectation.Skip);
        var validCase = CreateCase("valid-default", UseCaseCaseKind.Valid, UseCaseHandlerExpectation.Completes, UseCaseValidatorExpectation.Pass);

        var catalog = new UseCaseCaseCatalog
        {
            ValidCases = [validCase],
            BoundaryCases = [skipCase],
            InvalidCases = [],
            ExceptionCases = []
        };

        var handlerCases = catalog.HandlerCases().Select(entry => Assert.IsType<UseCaseCaseSet>(entry[0])).ToList();
        var validatorCases = catalog.ValidatorCases().Select(entry => Assert.IsType<UseCaseCaseSet>(entry[0])).ToList();

        Assert.Single(handlerCases);
        Assert.Single(validatorCases);
        Assert.Same(validCase, handlerCases[0]);
        Assert.Same(validCase, validatorCases[0]);
    }

    private static UseCaseCaseSet CreateCase(
        string name,
        UseCaseCaseKind kind,
        UseCaseHandlerExpectation handlerExpectation,
        UseCaseValidatorExpectation validatorExpectation)
    {
        return new UseCaseCaseSet
        {
            Name = name,
            Kind = kind,
            HandlerExpectation = handlerExpectation,
            ValidatorExpectation = validatorExpectation,
            TestCase = new UseCaseTestCase
            {
                Name = name,
                Scenario = new UseCaseScenario
                {
                    Request = new object()
                }
            }
        };
    }
}
