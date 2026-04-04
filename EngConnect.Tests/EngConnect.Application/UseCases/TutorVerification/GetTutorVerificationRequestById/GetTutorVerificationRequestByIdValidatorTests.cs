using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.GetTutorVerificationRequestById;

public class GetTutorVerificationRequestByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorVerificationRequestByIdTestData.Definition.ValidatorTypeFullName));
    }
}