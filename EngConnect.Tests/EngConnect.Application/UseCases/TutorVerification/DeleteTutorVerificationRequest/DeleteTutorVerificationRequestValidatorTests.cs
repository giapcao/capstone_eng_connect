using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest;

public class DeleteTutorVerificationRequestValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteTutorVerificationRequestTestData.Definition.ValidatorTypeFullName));
    }
}