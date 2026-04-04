using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ResetPassword;

public class ResetPasswordHandlerTests
{
    [Theory]
    [MemberData(nameof(ResetPasswordTestData.NormalHandlerCases), MemberType = typeof(ResetPasswordTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ResetPasswordTestData.Definition, caseSet);
    }
}