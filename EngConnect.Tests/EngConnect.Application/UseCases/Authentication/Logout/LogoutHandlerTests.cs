using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.Logout;

public class LogoutHandlerTests
{
    [Theory]
    [MemberData(nameof(LogoutTestData.NormalHandlerCases), MemberType = typeof(LogoutTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(LogoutTestData.Definition, caseSet);
    }
}