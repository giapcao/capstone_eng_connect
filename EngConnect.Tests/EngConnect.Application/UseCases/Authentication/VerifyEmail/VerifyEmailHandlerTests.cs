using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyEmail;

public class VerifyEmailHandlerTests
{
    [Theory]
    [MemberData(nameof(VerifyEmailTestData.NormalHandlerCases), MemberType = typeof(VerifyEmailTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(VerifyEmailTestData.Definition, caseSet);
    }
}