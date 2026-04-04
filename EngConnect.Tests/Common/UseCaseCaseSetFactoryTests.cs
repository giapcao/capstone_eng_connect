using Xunit;

namespace EngConnect.Tests.Common;

public class UseCaseCaseSetFactoryTests
{
    [Fact]
    public void CloneWithRequest_preserves_template_metadata_and_replaces_request()
    {
        var originalRequest = new SampleRequest
        {
            Value = "original"
        };
        var clonedRequest = new SampleRequest
        {
            Value = "cloned"
        };

        var template = new UseCaseCaseSet
        {
            Name = "valid-default",
            Kind = UseCaseCaseKind.Valid,
            HandlerExpectation = UseCaseHandlerExpectation.Completes,
            ValidatorExpectation = UseCaseValidatorExpectation.Pass,
            TestCase = new UseCaseTestCase
            {
                Name = "valid-default",
                Scenario = new UseCaseScenario
                {
                    Request = originalRequest,
                    Overrides = new Dictionary<Type, object?>
                    {
                        [typeof(string)] = "seed"
                    }
                }
            }
        };

        var cloned = UseCaseCaseSetFactory.CloneWithRequest(template, clonedRequest);

        Assert.Equal(template.Name, cloned.Name);
        Assert.Equal(template.Kind, cloned.Kind);
        Assert.Equal(template.HandlerExpectation, cloned.HandlerExpectation);
        Assert.Equal(template.ValidatorExpectation, cloned.ValidatorExpectation);
        Assert.Same(clonedRequest, cloned.TestCase.Scenario.Request);
        Assert.NotSame(template.TestCase.Scenario.Overrides, cloned.TestCase.Scenario.Overrides);
        Assert.Equal("seed", cloned.TestCase.Scenario.Overrides[typeof(string)]);
    }

    private sealed class SampleRequest
    {
        public required string Value { get; init; }
    }
}
