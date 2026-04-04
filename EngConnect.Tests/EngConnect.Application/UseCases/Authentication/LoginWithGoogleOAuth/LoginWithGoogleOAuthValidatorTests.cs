using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth;

public class LoginWithGoogleOAuthValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(LoginWithGoogleOAuthTestData.Definition.ValidatorTypeFullName));
    }
}