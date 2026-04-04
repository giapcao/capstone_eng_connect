using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin;

public class VerifyGoogleLoginValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(VerifyGoogleLoginTestData.Definition.ValidatorTypeFullName));
    }
}