using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginByUser;

public class LoginByUserValidatorTests
{
    [Theory]
    [MemberData(nameof(LoginByUserTestData.NormalValidatorCases), MemberType = typeof(LoginByUserTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(LoginByUserTestData.Definition, caseSet);
    }
}