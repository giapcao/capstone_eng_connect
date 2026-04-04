namespace EngConnect.Tests.Common;

internal static class UseCaseCaseSetFactory
{
    public static UseCaseCaseSet CloneWithRequest(UseCaseCaseSet template, object request)
    {
        return new UseCaseCaseSet
        {
            Name = template.Name,
            Kind = template.Kind,
            HandlerExpectation = template.HandlerExpectation,
            ValidatorExpectation = template.ValidatorExpectation,
            TestCase = new UseCaseTestCase
            {
                Name = template.TestCase.Name,
                Scenario = new UseCaseScenario
                {
                    Request = request,
                    Overrides = new Dictionary<Type, object?>(template.TestCase.Scenario.Overrides),
                    ArrangeMocks = template.TestCase.Scenario.ArrangeMocks,
                    AssertHandlerResultAsync = template.TestCase.Scenario.AssertHandlerResultAsync,
                    AssertValidatorResultAsync = template.TestCase.Scenario.AssertValidatorResultAsync
                }
            }
        };
    }
}
