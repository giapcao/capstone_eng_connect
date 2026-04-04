using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest;

public class GetListTutorVerificationRequestValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorVerificationRequestTestData.Definition.ValidatorTypeFullName));
    }
}