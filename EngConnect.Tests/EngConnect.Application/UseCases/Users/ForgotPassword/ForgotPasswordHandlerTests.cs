using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ForgotPassword;

public class ForgotPasswordHandlerTests
{
    [Theory]
    [MemberData(nameof(ForgotPasswordTestData.NormalHandlerCases), MemberType = typeof(ForgotPasswordTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ForgotPasswordTestData.Definition, caseSet);
    }
}