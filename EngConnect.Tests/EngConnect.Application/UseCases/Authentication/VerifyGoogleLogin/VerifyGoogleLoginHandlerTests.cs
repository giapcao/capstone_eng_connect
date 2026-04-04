using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin;

public class VerifyGoogleLoginHandlerTests
{
    [Theory]
    [MemberData(nameof(VerifyGoogleLoginTestData.NormalHandlerCases), MemberType = typeof(VerifyGoogleLoginTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(VerifyGoogleLoginTestData.Definition, caseSet);
    }
}