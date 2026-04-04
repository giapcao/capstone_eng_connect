using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginByUser;

public class LoginByUserHandlerTests
{
    [Theory]
    [MemberData(nameof(LoginByUserTestData.NormalHandlerCases), MemberType = typeof(LoginByUserTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(LoginByUserTestData.Definition, caseSet);
    }
}