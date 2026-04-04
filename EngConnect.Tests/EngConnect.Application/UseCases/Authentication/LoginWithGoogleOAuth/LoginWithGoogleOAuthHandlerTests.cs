using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth;

public class LoginWithGoogleOAuthHandlerTests
{
    [Theory]
    [MemberData(nameof(LoginWithGoogleOAuthTestData.NormalHandlerCases), MemberType = typeof(LoginWithGoogleOAuthTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(LoginWithGoogleOAuthTestData.Definition, caseSet);
    }
}