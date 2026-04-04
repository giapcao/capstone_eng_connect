using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ChangePassword;

public class ChangePasswordHandlerTests
{
    [Theory]
    [MemberData(nameof(ChangePasswordTestData.NormalHandlerCases), MemberType = typeof(ChangePasswordTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ChangePasswordTestData.Definition, caseSet);
    }
}