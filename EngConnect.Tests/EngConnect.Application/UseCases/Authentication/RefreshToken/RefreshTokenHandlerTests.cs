using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RefreshToken;

public class RefreshTokenHandlerTests
{
    [Theory]
    [MemberData(nameof(RefreshTokenTestData.NormalHandlerCases), MemberType = typeof(RefreshTokenTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RefreshTokenTestData.Definition, caseSet);
    }
}